using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Core.Sessions.Commands;

public class DeleteSessionsCommandHandler : IDeleteSessionsCommand
{
  private readonly ISessionRepository _sessionRepository;

  public DeleteSessionsCommandHandler(ISessionRepository sessionRepository)
  {
    _sessionRepository = sessionRepository;
  }

  public async Task ExecuteAsync(UserAggregate user, CancellationToken cancellationToken)
  {
    IEnumerable<SessionAggregate> sessions = await _sessionRepository.LoadAsync(user, cancellationToken);
    foreach (SessionAggregate session in sessions)
    {
      session.Delete();
    }

    await _sessionRepository.SaveAsync(sessions, cancellationToken);
  }
}
