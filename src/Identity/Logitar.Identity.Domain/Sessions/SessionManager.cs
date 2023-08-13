using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Sessions;

public class SessionManager : ISessionManager
{
  private readonly IAggregateRepository _aggregateRepository;

  public SessionManager(IAggregateRepository aggregateRepository)
  {
    _aggregateRepository = aggregateRepository;
  }

  public async Task DeleteAsync(SessionAggregate session, CancellationToken cancellationToken)
  {
    session.Delete();
    await _aggregateRepository.SaveAsync(session, cancellationToken);
  }

  public async Task SaveAsync(SessionAggregate session, CancellationToken cancellationToken)
    => await _aggregateRepository.SaveAsync(session, cancellationToken);
}
