using Logitar.EventSourcing;
using Logitar.Identity.Core.ApiKeys;
using Logitar.Identity.Core.ApiKeys.Models;
using Logitar.Identity.Core.ApiKeys.Payloads;
using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Payloads;
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

    _apiKey = new("Default", tenantId: Guid.NewGuid().ToString());
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
    Pbkdf2 pbkdf = Pbkdf2.Parse(entity.Secret);
    Assert.True(pbkdf.IsMatch(apiKey.Secret));
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

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _apiKeyRepository.SaveAsync(_apiKey);
  }
}
