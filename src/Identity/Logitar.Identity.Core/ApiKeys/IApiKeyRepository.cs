using Logitar.EventSourcing;
using Logitar.Identity.Domain.ApiKeys;

namespace Logitar.Identity.Core.ApiKeys;

public interface IApiKeyRepository
{
  Task<ApiKeyAggregate?> LoadAsync(AggregateId id, CancellationToken cancellationToken = default);

  Task SaveAsync(ApiKeyAggregate apiKey, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<ApiKeyAggregate> apiKeys, CancellationToken cancellationToken = default);
}
