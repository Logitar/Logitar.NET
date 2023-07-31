using Logitar.EventSourcing;
using Logitar.Identity.Core.Passwords;
using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Core.Users.Payloads;
using Logitar.Identity.Domain.Users;
using Logitar.Security;
using MediatR;

namespace Logitar.Identity.Core.Users.Commands;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, User?>
{
  private readonly IPasswordHelper _passwordHelper;
  private readonly IUserQuerier _userQuerier;
  private readonly IUserRepository _userRepository;

  public ChangePasswordCommandHandler(IPasswordHelper passwordHelper, IUserQuerier userQuerier,
    IUserRepository userRepository)
  {
    _passwordHelper = passwordHelper;
    _userQuerier = userQuerier;
    _userRepository = userRepository;
  }

  public async Task<User?> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
  {
    AggregateId userId = command.Id.GetAggregateId(nameof(command.Id));
    UserAggregate? user = await _userRepository.LoadAsync(userId, cancellationToken);
    if (user == null)
    {
      return null;
    }

    ChangePasswordPayload payload = command.Payload;
    Password password = _passwordHelper.Create(payload.Password);

    user.ChangePassword(payload.Current, password);

    await _userRepository.SaveAsync(user, cancellationToken);

    return await _userQuerier.ReadAsync(user, cancellationToken);
  }
}
