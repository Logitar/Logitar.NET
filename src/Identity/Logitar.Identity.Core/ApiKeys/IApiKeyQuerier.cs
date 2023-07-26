using Logitar.Identity.Core.ApiKeys.Models;
using Logitar.Identity.Domain.ApiKeys;

namespace Logitar.Identity.Core.ApiKeys;

public interface IApiKeyQuerier
{
  Task<ApiKey> ReadAsync(ApiKeyAggregate apiKey, CancellationToken cancellationToken = default);
  Task<ApiKey?> ReadAsync(string id, CancellationToken cancellationToken = default);
}
