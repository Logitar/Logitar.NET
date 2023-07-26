using Logitar.Identity.Core.ApiKeys;
using Logitar.Identity.Core.ApiKeys.Models;
using Logitar.Identity.Core.ApiKeys.Payloads;
using Logitar.Identity.Domain.ApiKeys;
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

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _apiKeyRepository.SaveAsync(_apiKey);
  }
}
