using Logitar.EventSourcing;
using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Core.Users.Payloads;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users;
using MediatR;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.Core.Users.Commands;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, User?>
{
  private readonly IUserQuerier _userQuerier;
  private readonly IUserRepository _userRepository;
  private readonly IOptions<UserSettings> _userSettings;

  public ChangePasswordCommandHandler(IUserQuerier userQuerier, IUserRepository userRepository,
    IOptions<UserSettings> userSettings)
  {
    _userQuerier = userQuerier;
    _userRepository = userRepository;
    _userSettings = userSettings;
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
    UserSettings userSettings = _userSettings.Value;

    user.ChangePassword(userSettings.PasswordSettings, payload.Password, payload.Current);

    await _userRepository.SaveAsync(user, cancellationToken);

    return await _userQuerier.ReadAsync(user, cancellationToken);
  }
}
