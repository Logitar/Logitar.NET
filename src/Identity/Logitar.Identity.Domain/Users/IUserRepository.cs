using Logitar.EventSourcing;
using Logitar.Identity.Domain.Roles;

namespace Logitar.Identity.Domain.Users;

public interface IUserRepository
{
  Task<UserAggregate?> LoadAsync(AggregateId id, CancellationToken cancellationToken = default);
  Task<UserAggregate?> LoadAsync(AggregateId id, long? version, CancellationToken cancellationToken = default);
  Task<UserAggregate?> LoadAsync(AggregateId id, bool includeDeleted, CancellationToken cancellationToken = default);
  Task<UserAggregate?> LoadAsync(AggregateId id, long? version, bool includeDeleted, CancellationToken cancellationToken = default);

  Task<IEnumerable<UserAggregate>> LoadAsync(CancellationToken cancellationToken = default);
  Task<IEnumerable<UserAggregate>> LoadAsync(bool includeDeleted, CancellationToken cancellationToken = default);

  Task<IEnumerable<UserAggregate>> LoadAsync(IEnumerable<AggregateId> ids, CancellationToken cancellationToken = default);
  Task<IEnumerable<UserAggregate>> LoadAsync(IEnumerable<AggregateId> ids, bool includeDeleted, CancellationToken cancellationToken = default);

  Task<IEnumerable<UserAggregate>> LoadAsync(string? tenantId, CancellationToken cancellationToken = default);
  Task<UserAggregate?> LoadAsync(string? tenantId, string uniqueName, CancellationToken cancellationToken = default);
  Task<IEnumerable<UserAggregate>> LoadAsync(string? tenantId, IEmailAddress email, CancellationToken cancellationToken = default);
  Task<IEnumerable<UserAggregate>> LoadAsync(RoleAggregate role, CancellationToken cancellationToken = default);
}
