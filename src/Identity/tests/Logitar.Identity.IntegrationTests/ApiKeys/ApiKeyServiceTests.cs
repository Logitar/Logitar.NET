using FluentValidation;
using FluentValidation.Results;
using Logitar.EventSourcing;
using Logitar.Identity.Core.ApiKeys;
using Logitar.Identity.Core.ApiKeys.Models;
using Logitar.Identity.Core.ApiKeys.Payloads;
using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Payloads;
using Logitar.Identity.Domain;
using Logitar.Identity.Domain.ApiKeys;
using Logitar.Identity.Domain.ApiKeys.Events;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Actors;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using Logitar.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.IntegrationTests.ApiKeys;

[Trait(Traits.Category, Categories.Integration)]
public class ApiKeyServiceTests : IntegrationTestingBase
{
  private readonly IApiKeyRepository _apiKeyRepository;
  private readonly IApiKeyService _apiKeyService;

  private readonly ApiKeyAggregate _apiKey;

  public ApiKeyServiceTests() : base()
  {
    _apiKeyRepository = ServiceProvider.GetRequiredService<IApiKeyRepository>();
    _apiKeyService = ServiceProvider.GetRequiredService<IApiKeyService>();

    _apiKey = new("Default", tenantId: Guid.NewGuid().ToString())
    {
      ExpiresOn = DateTime.UtcNow.AddYears(1)
    };
  }

  [Fact(DisplayName = "CreateAsync: it should create the correct API key.")]
  public async Task CreateAsync_it_should_create_the_correct_api_key()
  {
    CreateApiKeyPayload payload = new()
    {
      TenantId = _apiKey.TenantId,
      Title = "  Title  ",
      Description = "    ",
      ExpiresOn = DateTime.UtcNow.AddYears(1)
    };
    ApiKey apiKey = await _apiKeyService.CreateAsync(payload, CancellationToken);
    Assert.Equal(payload.TenantId, apiKey.TenantId);
    Assert.Equal(payload.Title.Trim(), apiKey.Title);
    Assert.Equal(payload.ExpiresOn, apiKey.ExpiresOn);
    Assert.NotNull(apiKey.Secret);
    Assert.Null(apiKey.Description);

    ApiKeyEntity? entity = await IdentityContext.ApiKeys.SingleOrDefaultAsync(x => x.AggregateId == apiKey.Id);
    Assert.NotNull(entity);
    Pbkdf2 pbkdf2 = Pbkdf2.Parse(entity.Secret);
    Assert.True(pbkdf2.IsMatch(apiKey.Secret));
  }

  [Fact(DisplayName = "DeleteAsync: it should delete the correct API key.")]
  public async Task DeleteAsync_it_should_delete_the_correct_api_key()
  {
    ApiKey? apiKey = await _apiKeyService.DeleteAsync(_apiKey.Id.Value, CancellationToken);
    Assert.NotNull(apiKey);
    Assert.Equal(_apiKey.Id.Value, apiKey.Id);

    ApiKeyEntity? entity = await IdentityContext.ApiKeys.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == _apiKey.Id.Value);
    Assert.Null(entity);
  }

  [Fact(DisplayName = "DeleteAsync: it should return null when API key is not found.")]
  public async Task DeleteAsync_it_should_return_null_when_api_key_is_not_found()
  {
    ApiKey? apiKey = await _apiKeyService.DeleteAsync(Guid.Empty.ToString(), CancellationToken);
    Assert.Null(apiKey);
  }

  [Fact(DisplayName = "It should throw CannotPostponeApiKeyExpirationException when postponing the expiration.")]
  public async Task It_should_throw_CannotPostponeApiKeyExpirationException_when_postponing_the_expiration()
  {
    Assert.True(_apiKey.ExpiresOn.HasValue);
    UpdateApiKeyPayload payload = new()
    {
      ExpiresOn = _apiKey.ExpiresOn.Value.AddMonths(6)
    };
    var exception = await Assert.ThrowsAsync<CannotPostponeApiKeyExpirationException>(
      async () => await _apiKeyService.UpdateAsync(_apiKey.Id.Value, payload, CancellationToken));
    Assert.Equal(_apiKey.ToString(), exception.ApiKey);
    Assert.Equal(payload.ExpiresOn, exception.ExpiresOn);
    Assert.Equal("ExpiresOn", exception.PropertyName);
  }

  [Fact(DisplayName = "It should throw ValidationException when expiration is not in the future.")]
  public async Task It_should_throw_ValidationException_when_expiration_is_not_in_the_future()
  {
    CreateApiKeyPayload payload = new()
    {
      TenantId = _apiKey.TenantId,
      Title = _apiKey.Title,
      Description = _apiKey.Description,
      ExpiresOn = DateTime.UtcNow.AddDays(-10)
    };
    var exception = await Assert.ThrowsAsync<ValidationException>(
      async () => await _apiKeyService.CreateAsync(payload, CancellationToken));
    ValidationFailure failure = exception.Errors.Single();
    Assert.Equal("FutureValidator", failure.ErrorCode);
    Assert.Equal("ExpiresOn", failure.PropertyName);
  }

  [Fact(DisplayName = "ReadAsync: it should read the correct API key.")]
  public async Task ReadAsync_it_should_read_the_correct_api_key()
  {
    ApiKey? apiKey = await _apiKeyService.ReadAsync(id: _apiKey.Id.Value, cancellationToken: CancellationToken);
    Assert.NotNull(apiKey);
    Assert.Equal(_apiKey.Id.Value, apiKey.Id);
  }

  [Fact(DisplayName = "ReadAsync: it should return null when API key is not found.")]
  public async Task ReadAsync_it_should_return_null_when_api_key_is_not_found()
  {
    ApiKey? apiKey = await _apiKeyService.ReadAsync(id: Guid.Empty.ToString(), cancellationToken: CancellationToken);
    Assert.Null(apiKey);
  }

  [Fact(DisplayName = "SearchAsync: it should return the correct search results.")]
  public async Task SearchAsync_it_should_return_the_correct_search_results()
  {
    string tenantId = Guid.NewGuid().ToString();

    ApiKeyAggregate apiKey1 = new("API Key #1", tenantId);
    ApiKeyAggregate apiKey2 = new("API Key #2", tenantId);
    ApiKeyAggregate apiKey3 = new("API Key #3", tenantId);
    await _apiKeyRepository.SaveAsync(new[] { apiKey1, apiKey2, apiKey3 });

    ActorEntity actor = ActorEntity.From(new Actor());
    AggregateId expiredId = AggregateId.NewId();
    ApiKeyCreatedEvent created = new()
    {
      Id = Guid.NewGuid(),
      AggregateId = expiredId,
      Version = 1,
      OccurredOn = DateTime.UtcNow,
      Secret = new Pbkdf2(RandomNumberGenerator.GetBytes(32)),
      TenantId = tenantId,
      Title = "API Key #0"
    };
    ApiKeyUpdatedEvent updated = new()
    {
      Id = Guid.NewGuid(),
      AggregateId = expiredId,
      Version = 2,
      OccurredOn = DateTime.UtcNow,
      ExpiresOn = DateTime.UtcNow.AddDays(-1)
    };
    ApiKeyEntity expired = new(created, actor);
    expired.Update(updated, actor);
    IdentityContext.ApiKeys.Add(expired);
    await IdentityContext.SaveChangesAsync();

    SearchApiKeyPayload payload = new()
    {
      IsExpired = false,
      Sort = new[]
      {
        new ApiKeySortOption((ApiKeySort)(-1)),
        new ApiKeySortOption(ApiKeySort.Title, isDescending: false)
      },
      Skip = -5,
      Limit = 2
    };
    payload.Id.Operator = (SearchOperator)(-1);
    payload.Id.Terms = new[] { new SearchTerm(_apiKey.Id.Value) };
    payload.Search.Terms = new[] { new SearchTerm("%API Key%") };
    payload.TenantId.Terms = new[] { new SearchTerm(tenantId) };

    SearchResults<ApiKey> results = await _apiKeyService.SearchAsync(payload, CancellationToken);
    Assert.Equal(3, results.Total);
    Assert.Equal(2, results.Items.Count());
    Assert.Equal(apiKey1.Id.Value, results.Items.ElementAt(0).Id);
    Assert.Equal(apiKey2.Id.Value, results.Items.ElementAt(1).Id);
  }

  [Fact(DisplayName = "ReplaceAsync: it should replace the correct API key.")]
  public async Task ReplaceAsync_it_should_replace_the_correct_api_key()
  {
    ReplaceApiKeyPayload payload = new()
    {
      Title = "  Default  ",
      Description = "This is the default API key.",
      ExpiresOn = _apiKey.ExpiresOn?.AddMonths(-6)
    };
    ApiKey? apiKey = await _apiKeyService.ReplaceAsync(_apiKey.Id.Value, payload, CancellationToken);
    Assert.NotNull(apiKey);
    Assert.Equal(_apiKey.Title, apiKey.Title);
    Assert.Equal(payload.Description, apiKey.Description);
    Assert.Equal(payload.ExpiresOn, apiKey.ExpiresOn);
  }

  [Fact(DisplayName = "ReplaceAsync: it should return null when API key is not found.")]
  public async Task ReplaceAsync_it_should_return_null_when_api_key_is_not_found()
  {
    ReplaceApiKeyPayload payload = new();
    ApiKey? apiKey = await _apiKeyService.ReplaceAsync(Guid.Empty.ToString(), payload, CancellationToken);
    Assert.Null(apiKey);
  }

  [Fact(DisplayName = "UpdateAsync: it should return null when API key is not found.")]
  public async Task UpdateAsync_it_should_return_null_when_api_key_is_not_found()
  {
    UpdateApiKeyPayload payload = new();
    ApiKey? apiKey = await _apiKeyService.UpdateAsync(Guid.Empty.ToString(), payload, CancellationToken);
    Assert.Null(apiKey);
  }

  [Fact(DisplayName = "UpdateAsync: it should update the correct API key.")]
  public async Task UpdateAsync_it_should_update_the_correct_api_key()
  {
    UpdateApiKeyPayload payload = new()
    {
      Title = "  Default  ",
      Description = new MayBe<string>("This is the default API key."),
      ExpiresOn = _apiKey.ExpiresOn?.AddMonths(-6)
    };
    ApiKey? apiKey = await _apiKeyService.UpdateAsync(_apiKey.Id.Value, payload, CancellationToken);
    Assert.NotNull(apiKey);
    Assert.Equal(_apiKey.Title, apiKey.Title);
    Assert.Equal(payload.Description.Value, apiKey.Description);
    Assert.Equal(payload.ExpiresOn, apiKey.ExpiresOn);
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _apiKeyRepository.SaveAsync(_apiKey);
  }
}
