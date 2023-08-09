using Logitar.EventSourcing;
using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Domain.Roles;

public interface IRoleRepository
{
  Task<RoleAggregate?> LoadAsync(AggregateId id, CancellationToken cancellationToken = default);
  Task<RoleAggregate?> LoadAsync(AggregateId id, long? version, CancellationToken cancellationToken = default);
  Task<RoleAggregate?> LoadAsync(AggregateId id, bool includeDeleted, CancellationToken cancellationToken = default);
  Task<RoleAggregate?> LoadAsync(AggregateId id, long? version, bool includeDeleted, CancellationToken cancellationToken = default);

  Task<IEnumerable<RoleAggregate>> LoadAsync(CancellationToken cancellationToken = default);
  Task<IEnumerable<RoleAggregate>> LoadAsync(bool includeDeleted, CancellationToken cancellationToken = default);

  Task<IEnumerable<RoleAggregate>> LoadAsync(IEnumerable<AggregateId> ids, CancellationToken cancellationToken = default);
  Task<IEnumerable<RoleAggregate>> LoadAsync(IEnumerable<AggregateId> ids, bool includeDeleted, CancellationToken cancellationToken = default);

  Task<IEnumerable<RoleAggregate>> LoadAsync(string? tenantId, CancellationToken cancellationToken = default);
  Task<RoleAggregate?> LoadAsync(string? tenantId, string uniqueName, CancellationToken cancellationToken = default);
  Task<IEnumerable<RoleAggregate>> LoadAsync(UserAggregate user, CancellationToken cancellationToken = default);

  Task SaveAsync(RoleAggregate role, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<RoleAggregate> roles, CancellationToken cancellationToken = default);
}
