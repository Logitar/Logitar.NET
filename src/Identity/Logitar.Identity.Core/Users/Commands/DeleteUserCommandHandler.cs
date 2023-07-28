using Logitar.EventSourcing;
using Logitar.Identity.Core.Sessions.Commands;
using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Domain.Users;
using MediatR;

namespace Logitar.Identity.Core.Users.Commands;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, User?>
{
  private readonly IMediator _mediator;
  private readonly IUserQuerier _userQuerier;
  private readonly IUserRepository _userRepository;

  public DeleteUserCommandHandler(IMediator mediator, IUserQuerier userQuerier,
    IUserRepository userRepository)
  {
    _mediator = mediator;
    _userQuerier = userQuerier;
    _userRepository = userRepository;
  }

  public async Task<User?> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
  {
    AggregateId userId = command.Id.GetAggregateId(nameof(command.Id));
    UserAggregate? user = await _userRepository.LoadAsync(userId, cancellationToken);
    if (user == null)
    {
      return null;
    }
    User result = await _userQuerier.ReadAsync(user, cancellationToken);

    user.Delete();

    await _mediator.Send(new DeleteUserSessionsCommand(user), cancellationToken);

    await _userRepository.SaveAsync(user, cancellationToken);

    return result;
  }
}
