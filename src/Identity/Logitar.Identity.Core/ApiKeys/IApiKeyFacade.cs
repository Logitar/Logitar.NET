using Logitar.Identity.Core.ApiKeys.Models;
using Logitar.Identity.Core.ApiKeys.Payloads;
using Logitar.Identity.Core.Models;

namespace Logitar.Identity.Core.ApiKeys;

public interface IApiKeyFacade
{
  Task<ApiKey> AuthenticateAsync(AuthenticateApiKeyPayload payload, CancellationToken cancellationToken = default);
  Task<ApiKey> CreateAsync(CreateApiKeyPayload payload, CancellationToken cancellationToken = default);
  Task<ApiKey?> DeleteAsync(string id, CancellationToken cancellationToken = default);
  Task<ApiKey?> ReadAsync(string? id = null, CancellationToken cancellationToken = default);
  Task<ApiKey?> ReplaceAsync(string id, ReplaceApiKeyPayload payload, CancellationToken cancellationToken = default);
  Task<SearchResults<ApiKey>> SearchAsync(SearchApiKeyPayload payload, CancellationToken cancellationToken = default);
  Task<ApiKey?> UpdateAsync(string id, UpdateApiKeyPayload payload, CancellationToken cancellationToken = default);
}
