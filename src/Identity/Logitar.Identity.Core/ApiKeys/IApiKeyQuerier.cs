using Logitar.Identity.Core.ApiKeys.Models;
using Logitar.Identity.Core.ApiKeys.Payloads;
using Logitar.Identity.Core.Models;
using Logitar.Identity.Domain.ApiKeys;

namespace Logitar.Identity.Core.ApiKeys;

public interface IApiKeyQuerier
{
  Task<ApiKey> ReadAsync(ApiKeyAggregate apiKey, CancellationToken cancellationToken = default);
  Task<ApiKey?> ReadAsync(string id, CancellationToken cancellationToken = default);
  Task<SearchResults<ApiKey>> SearchAsync(SearchApiKeyPayload payload, CancellationToken cancellationToken = default);
}
