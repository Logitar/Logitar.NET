using Logitar.EventSourcing;
using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Domain.Users;
using MediatR;

namespace Logitar.Identity.Core.Users.Commands;

public class SaveExternalIdentifierCommandHandler : IRequestHandler<SaveExternalIdentifierCommand, User?>
{
  private readonly IUserQuerier _userQuerier;
  private readonly IUserRepository _userRepository;

  public SaveExternalIdentifierCommandHandler(IUserQuerier userQuerier, IUserRepository userRepository)
  {
    _userQuerier = userQuerier;
    _userRepository = userRepository;
  }

  public async Task<User?> Handle(SaveExternalIdentifierCommand command, CancellationToken cancellationToken)
  {
    AggregateId userId = command.Id.GetAggregateId(nameof(command.Id));
    UserAggregate? user = await _userRepository.LoadAsync(userId, cancellationToken);
    if (user == null)
    {
      return null;
    }

    UserAggregate? other = await _userRepository.LoadAsync(user.TenantId, command.Key, command.Value, cancellationToken);
    if (other?.Equals(user) == false)
    {
      throw new ExternalIdentifierAlreadyUsedException(user.TenantId, command.Key, command.Value);
    }

    user.SetExternalIdentifier(command.Key, command.Value);

    await _userRepository.SaveAsync(user, cancellationToken);

    return await _userQuerier.ReadAsync(user, cancellationToken);
  }
}
