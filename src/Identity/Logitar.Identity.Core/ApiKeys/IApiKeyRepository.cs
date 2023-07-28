using Logitar.EventSourcing;
using Logitar.Identity.Domain.ApiKeys;
using Logitar.Identity.Domain.Roles;

namespace Logitar.Identity.Core.ApiKeys;

public interface IApiKeyRepository
{
  Task<ApiKeyAggregate?> LoadAsync(AggregateId id, CancellationToken cancellationToken = default);
  Task<IEnumerable<ApiKeyAggregate>> LoadAsync(RoleAggregate role, CancellationToken cancellationToken = default);

  Task SaveAsync(ApiKeyAggregate apiKey, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<ApiKeyAggregate> apiKeys, CancellationToken cancellationToken = default);
}
