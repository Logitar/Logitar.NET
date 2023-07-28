using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Users;
using MediatR;

namespace Logitar.Identity.Core.Sessions.Commands;

public class DeleteUserSessionsCommandHandler : IRequestHandler<DeleteUserSessionsCommand>
{
  private readonly ISessionRepository _sessionRepository;

  public DeleteUserSessionsCommandHandler(ISessionRepository sessionRepository)
  {
    _sessionRepository = sessionRepository;
  }

  public async Task Handle(DeleteUserSessionsCommand command, CancellationToken cancellationToken)
  {
    UserAggregate user = command.User;

    IEnumerable<SessionAggregate> sessions = await _sessionRepository.LoadAsync(user, cancellationToken);
    foreach (SessionAggregate session in sessions)
    {
      session.Delete();
    }

    await _sessionRepository.SaveAsync(sessions, cancellationToken);
  }
}
