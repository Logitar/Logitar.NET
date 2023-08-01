using Logitar.EventSourcing;
using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Domain.Users;
using MediatR;

namespace Logitar.Identity.Core.Users.Commands;

public class RemoveExternalIdentifierCommandHandler : IRequestHandler<RemoveExternalIdentifierCommand, User?>
{
  private readonly IUserQuerier _userQuerier;
  private readonly IUserRepository _userRepository;

  public RemoveExternalIdentifierCommandHandler(IUserQuerier userQuerier, IUserRepository userRepository)
  {
    _userQuerier = userQuerier;
    _userRepository = userRepository;
  }

  public async Task<User?> Handle(RemoveExternalIdentifierCommand command, CancellationToken cancellationToken)
  {
    AggregateId userId = command.Id.GetAggregateId(nameof(command.Id));
    UserAggregate? user = await _userRepository.LoadAsync(userId, cancellationToken);
    if (user == null)
    {
      return null;
    }

    user.RemoveExternalIdentifier(command.Key);

    await _userRepository.SaveAsync(user, cancellationToken);

    return await _userQuerier.ReadAsync(user, cancellationToken);
  }
}
