using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Core.Passwords;
using Logitar.Identity.Core.Tokens.Commands;
using Logitar.Identity.Core.Tokens.Models;
using Logitar.Identity.Core.Tokens.Payloads;
using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Core.Users.Payloads;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.Domain.Users.Validators;
using Logitar.Security;
using MediatR;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.Core.Users.Commands;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, User?>
{
  private readonly IMediator _mediator;
  private readonly IPasswordHelper _passwordHelper;
  private readonly IUserRepository _userRepository;
  private readonly IUserQuerier _userQuerier;
  private readonly IOptions<UserSettings> _userSettings;

  public ResetPasswordCommandHandler(IMediator mediator, IPasswordHelper passwordHelper,
    IUserRepository userRepository, IUserQuerier userQuerier, IOptions<UserSettings> userSettings)
  {
    _mediator = mediator;
    _passwordHelper = passwordHelper;
    _userRepository = userRepository;
    _userQuerier = userQuerier;
    _userSettings = userSettings;
  }

  public async Task<User?> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
  {
    ResetPasswordPayload payload = command.Payload;
    UserSettings userSettings = _userSettings.Value;

    new PasswordValidator(userSettings.PasswordSettings, nameof(payload.Password)).ValidateAndThrow(payload.Password);

    ValidateTokenPayload validateToken = new()
    {
      Token = payload.Token,
      Secret = payload.Secret,
      Audience = payload.Audience,
      Issuer = payload.Issuer,
      Purpose = userSettings.PasswordResetSettings.Purpose
    };
    ValidatedToken validatedToken = await _mediator.Send(new ValidateTokenCommand(validateToken, consume: true), cancellationToken);
    if (validatedToken.Subject == null)
    {
      return null;
    }

    AggregateId userId = new(validatedToken.Subject);
    UserAggregate? user = await _userRepository.LoadAsync(userId, cancellationToken);
    if (user == null)
    {
      return null;
    }

    Password password = _passwordHelper.Create(payload.Password);
    user.ResetPassword(password);

    await _userRepository.SaveAsync(user, cancellationToken);

    return await _userQuerier.ReadAsync(user, cancellationToken);
  }
}
