using Logitar.Identity.Core.Tokens.Commands;
using Logitar.Identity.Core.Tokens.Models;
using Logitar.Identity.Core.Tokens.Payloads;
using Logitar.Identity.Core.Users.Payloads;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users;
using MediatR;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.Core.Users.Commands;

public class RecoverPasswordCommandHandler : IRequestHandler<RecoverPasswordCommand, CreatedToken?>
{
  private readonly IMediator _mediator;
  private readonly IUserRepository _userRepository;
  private readonly IOptions<UserSettings> _userSettings;

  public RecoverPasswordCommandHandler(IMediator mediator, IUserRepository userRepository,
    IOptions<UserSettings> userSettings)
  {
    _mediator = mediator;
    _userRepository = userRepository;
    _userSettings = userSettings;
  }

  public async Task<CreatedToken?> Handle(RecoverPasswordCommand command, CancellationToken cancellationToken)
  {
    RecoverPasswordPayload payload = command.Payload;
    UserSettings userSettings = _userSettings.Value;

    UserAggregate? user = await _userRepository.LoadAsync(payload.TenantId, payload.UniqueName, cancellationToken);
    if (user == null)
    {
      if (userSettings.RequireUniqueEmail)
      {
        EmailAddress email = new(payload.UniqueName);
        IEnumerable<UserAggregate> users = await _userRepository.LoadAsync(payload.TenantId, email, cancellationToken);
        if (users.Count() <= 1)
        {
          user = users.SingleOrDefault();
        }
      }

      if (user == null)
      {
        return null;
      }
    }

    CreateTokenPayload createToken = new()
    {
      IsConsumable = true,
      Lifetime = userSettings.PasswordResetSettings.Lifetime,
      Purpose = userSettings.PasswordResetSettings.Purpose,
      Secret = payload.Secret,
      Algorithm = payload.Algorithm,
      Audience = payload.Audience,
      Issuer = payload.Issuer,
      Subject = user.Id.Value
    };
    CreatedToken createdToken = await _mediator.Send(new CreateTokenCommand(createToken), cancellationToken);

    return createdToken;
  }
}
