using Logitar.EventSourcing;
using Logitar.Identity.Domain.Sessions;

namespace Logitar.Identity.Core.Sessions;

public interface ISessionRepository
{
  Task<SessionAggregate?> LoadAsync(AggregateId id, CancellationToken cancellationToken = default);
  Task SaveAsync(SessionAggregate session, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<SessionAggregate> sessions, CancellationToken cancellationToken = default);
}
