using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Passwords;
using Logitar.Identity.Core.Sessions.Models;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users;
using Logitar.Security.Cryptography;
using MediatR;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.Core.Sessions.Commands;

public class SignInUserCommandHandler : IRequestHandler<SignInUserCommand, Session>
{
  private readonly IPasswordHelper _passwordHelper;
  private readonly ISessionQuerier _sessionQuerier;
  private readonly ISessionRepository _sessionRepository;
  private readonly IUserRepository _userRepository;
  private readonly IOptions<UserSettings> _userSettings;

  public SignInUserCommandHandler(IPasswordHelper passwordHelper, ISessionQuerier sessionQuerier,
    ISessionRepository sessionRepository, IUserRepository userRepository,
    IOptions<UserSettings> userSettings)
  {
    _passwordHelper = passwordHelper;
    _sessionQuerier = sessionQuerier;
    _sessionRepository = sessionRepository;
    _userRepository = userRepository;
    _userSettings = userSettings;
  }

  public async Task<Session> Handle(SignInUserCommand command, CancellationToken cancellationToken)
  {
    UserAggregate user = command.User;
    UserSettings userSettings = _userSettings.Value;

    byte[]? secretBytes = null;
    Password? secret = command.IsPersistent
      ? _passwordHelper.Generate(SessionAggregate.SecretLength, out secretBytes) : null;
    SessionAggregate session = user.SignIn(userSettings, command.Password, secret);

    foreach (CustomAttribute customAttribute in command.CustomAttributes)
    {
      session.SetCustomAttribute(customAttribute.Key, customAttribute.Value);
    }

    await _userRepository.SaveAsync(user, cancellationToken);
    await _sessionRepository.SaveAsync(session, cancellationToken);

    Session result = await _sessionQuerier.ReadAsync(session, cancellationToken);
    if (secretBytes != null)
    {
      result.RefreshToken = new RefreshToken(session.Id, secretBytes).ToString();
    }

    return result;
  }
}
