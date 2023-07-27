using Logitar.EventSourcing;
using Logitar.Identity.Domain.Roles;

namespace Logitar.Identity.Core.Roles;

public interface IRoleRepository
{
  Task<RoleAggregate?> LoadAsync(AggregateId id, CancellationToken cancellationToken = default);
  Task<RoleAggregate?> LoadAsync(string? tenantId, string uniqueName, CancellationToken cancellationToken = default);

  Task SaveAsync(RoleAggregate role, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<RoleAggregate> roles, CancellationToken cancellationToken = default);
}
