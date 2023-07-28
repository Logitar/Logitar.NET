using Logitar.EventSourcing;
using Logitar.Identity.Core.Sessions;
using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Users;
using MediatR;

namespace Logitar.Identity.Core.Users.Commands;

public class SignOutUserCommandHandler : IRequestHandler<SignOutUserCommand, User?>
{
  private readonly ISessionRepository _sessionRepository;
  private readonly IUserQuerier _userQuerier;
  private readonly IUserRepository _userRepository;

  public SignOutUserCommandHandler(ISessionRepository sessionRepository, IUserQuerier userQuerier,
    IUserRepository userRepository)
  {
    _sessionRepository = sessionRepository;
    _userQuerier = userQuerier;
    _userRepository = userRepository;
  }

  public async Task<User?> Handle(SignOutUserCommand command, CancellationToken cancellationToken)
  {
    AggregateId userId = command.Id.GetAggregateId(nameof(command.Id));
    UserAggregate? user = await _userRepository.LoadAsync(userId, cancellationToken);
    if (user == null)
    {
      return null;
    }

    IEnumerable<SessionAggregate> sessions = await _sessionRepository.LoadActiveAsync(user, cancellationToken);
    foreach (SessionAggregate session in sessions)
    {
      session.SignOut();
    }

    await _sessionRepository.SaveAsync(sessions, cancellationToken);

    return await _userQuerier.ReadAsync(user, cancellationToken);
  }
}
