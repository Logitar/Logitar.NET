using Logitar.EventSourcing;
using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Domain.Sessions;

public interface ISessionRepository
{
  Task<SessionAggregate?> LoadAsync(AggregateId id, CancellationToken cancellationToken = default);
  Task<SessionAggregate?> LoadAsync(AggregateId id, long? version, CancellationToken cancellationToken = default);
  Task<SessionAggregate?> LoadAsync(AggregateId id, bool includeDeleted, CancellationToken cancellationToken = default);
  Task<SessionAggregate?> LoadAsync(AggregateId id, long? version, bool includeDeleted, CancellationToken cancellationToken = default);

  Task<IEnumerable<SessionAggregate>> LoadAsync(CancellationToken cancellationToken = default);
  Task<IEnumerable<SessionAggregate>> LoadAsync(bool includeDeleted, CancellationToken cancellationToken = default);

  Task<IEnumerable<SessionAggregate>> LoadAsync(IEnumerable<AggregateId> ids, CancellationToken cancellationToken = default);
  Task<IEnumerable<SessionAggregate>> LoadAsync(IEnumerable<AggregateId> ids, bool includeDeleted, CancellationToken cancellationToken = default);

  Task<IEnumerable<SessionAggregate>> LoadAsync(string? tenantId, CancellationToken cancellationToken = default);
  Task<IEnumerable<SessionAggregate>> LoadAsync(UserAggregate user, CancellationToken cancellationToken = default);
  Task<IEnumerable<SessionAggregate>> LoadAsync(UserAggregate user, bool? isActive, CancellationToken cancellationToken = default);
}
