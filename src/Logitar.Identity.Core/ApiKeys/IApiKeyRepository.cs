using Logitar.Identity.Core.ApiKeys.Models;
using Logitar.Identity.Core.Roles;

namespace Logitar.Identity.Core.ApiKeys;

/// <summary>
/// Represents an interface that allows retrieving and storing API keys in an event store.
/// </summary>
public interface IApiKeyRepository
{
  /// <summary>
  /// Loads a list of API keys from the event store.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant in which the API keys belongs to.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The loaded API keys.</returns>
  Task<IEnumerable<RoleAggregate>> LoadAsync(string? tenantId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Persists an API key to the event store.
  /// </summary>
  /// <param name="apiKey">The API key to persist.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task SaveAsync(ApiKeyAggregate apiKey, CancellationToken cancellationToken = default);
  /// <summary>
  /// Persists a list of API keys to the event store.
  /// </summary>
  /// <param name="apiKeys">The list of API keys to persist.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task SaveAsync(IEnumerable<ApiKeyAggregate> apiKeys, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves the read model of the specified API key.
  /// </summary>
  /// <param name="apiKey">The API key to retrieve its read model.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The read model.</returns>
  Task<ApiKey> ReadAsync(ApiKeyAggregate apiKey, CancellationToken cancellationToken = default);
}
