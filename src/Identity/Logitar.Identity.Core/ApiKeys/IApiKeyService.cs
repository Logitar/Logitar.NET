using Logitar.Identity.Core.ApiKeys.Models;
using Logitar.Identity.Core.ApiKeys.Payloads;
using Logitar.Identity.Core.Models;

namespace Logitar.Identity.Core.ApiKeys;

public interface IApiKeyService
{
  Task<ApiKey> CreateAsync(CreateApiKeyPayload payload, CancellationToken cancellationToken = default);
  Task<ApiKey?> DeleteAsync(string id, CancellationToken cancellationToken = default);
  Task<ApiKey?> ReadAsync(string? id = null, CancellationToken cancellationToken = default);
  Task<SearchResults<ApiKey>> SearchAsync(SearchApiKeyPayload payload, CancellationToken cancellationToken = default);
}
