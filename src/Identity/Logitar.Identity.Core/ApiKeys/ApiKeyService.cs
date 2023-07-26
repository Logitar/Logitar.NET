using Logitar.EventSourcing;
using Logitar.Identity.Core.ApiKeys.Models;
using Logitar.Identity.Core.ApiKeys.Payloads;
using Logitar.Identity.Domain.ApiKeys;

namespace Logitar.Identity.Core.ApiKeys;

public class ApiKeyService : IApiKeyService
{
  private readonly IApiKeyQuerier _apiKeyQuerier;
  private readonly IApiKeyRepository _apiKeyRepository;

  public ApiKeyService(IApiKeyQuerier apiKeyQuerier, IApiKeyRepository apiKeyRepository)
  {
    _apiKeyQuerier = apiKeyQuerier;
    _apiKeyRepository = apiKeyRepository;
  }

  public async Task<ApiKey> CreateAsync(CreateApiKeyPayload payload, CancellationToken cancellationToken)
  {
    ApiKeyAggregate apiKey = new(payload.Title, payload.TenantId)
    {
      Description = payload.Description,
      ExpiresOn = payload.ExpiresOn
    };

    await _apiKeyRepository.SaveAsync(apiKey, cancellationToken);

    ApiKey result = await _apiKeyQuerier.ReadAsync(apiKey, cancellationToken);
    if (apiKey.Secret != null)
    {
      result.Secret = apiKey.Secret;
    }

    return result;
  }

  public async Task<ApiKey?> DeleteAsync(string id, CancellationToken cancellationToken)
  {

    AggregateId apiKeyId = id.GetAggregateId(nameof(id));
    ApiKeyAggregate? apiKey = await _apiKeyRepository.LoadAsync(apiKeyId, cancellationToken);
    if (apiKey == null)
    {
      return null;
    }
    ApiKey result = await _apiKeyQuerier.ReadAsync(apiKey, cancellationToken);

    apiKey.Delete();

    await _apiKeyRepository.SaveAsync(apiKey, cancellationToken);

    return result;
  }
}
