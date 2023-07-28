using Logitar.Identity.Core.Sessions.Models;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users;
using MediatR;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.Core.Sessions.Commands;

public class SignInUserCommandHandler : IRequestHandler<SignInUserCommand, Session>
{
  private readonly ISessionQuerier _sessionQuerier;
  private readonly ISessionRepository _sessionRepository;
  private readonly IUserRepository _userRepository;
  private readonly IOptions<UserSettings> _userSettings;

  public SignInUserCommandHandler(ISessionQuerier sessionQuerier,
    ISessionRepository sessionRepository, IUserRepository userRepository,
    IOptions<UserSettings> userSettings)
  {
    _sessionQuerier = sessionQuerier;
    _sessionRepository = sessionRepository;
    _userRepository = userRepository;
    _userSettings = userSettings;
  }

  public async Task<Session> Handle(SignInUserCommand command, CancellationToken cancellationToken)
  {
    UserAggregate user = command.User;
    UserSettings userSettings = _userSettings.Value;

    SessionAggregate session = user.SignIn(userSettings, command.Password, command.IsPersistent);

    await _userRepository.SaveAsync(user, cancellationToken);
    await _sessionRepository.SaveAsync(session, cancellationToken);

    Session result = await _sessionQuerier.ReadAsync(session, cancellationToken);
    if (session.Secret != null)
    {
      result.RefreshToken = new RefreshToken(session).ToString();
    }

    return result;
  }
}
