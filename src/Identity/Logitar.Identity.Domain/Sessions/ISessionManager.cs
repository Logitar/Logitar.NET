namespace Logitar.Identity.Domain.Sessions;

public interface ISessionManager
{
  Task DeleteAsync(SessionAggregate session, CancellationToken cancellationToken = default);
  Task SaveAsync(SessionAggregate session, CancellationToken cancellationToken = default);
}
