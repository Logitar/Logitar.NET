using Logitar.EventSourcing;
using Logitar.Identity.Core.Sessions.Models;
using Logitar.Identity.Core.Sessions.Payloads;
using Logitar.Identity.Domain;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.Core.Sessions;

public class SessionService : ISessionService
{
  private readonly ISessionQuerier _sessionQuerier;
  private readonly ISessionRepository _sessionRepository;
  private readonly IUserRepository _userRepository;
  private readonly IOptions<UserSettings> _userSettings;

  public SessionService(ISessionQuerier sessionQuerier, ISessionRepository sessionRepository,
    IUserRepository userRepository, IOptions<UserSettings> userSettings)
  {
    _sessionQuerier = sessionQuerier;
    _sessionRepository = sessionRepository;
    _userRepository = userRepository;
    _userSettings = userSettings;
  }

  public virtual async Task<Session> CreateAsync(CreateSessionPayload payload, CancellationToken cancellationToken)
  {
    AggregateId userId = payload.UserId.GetAggregateId(nameof(payload.UserId));
    UserAggregate user = await _userRepository.LoadAsync(userId, cancellationToken)
      ?? throw new AggregateNotFoundException<UserAggregate>(payload.UserId, nameof(payload.UserId));

    return await SignInAsync(user, password: null, payload.IsPersistent, cancellationToken);
  }

  public virtual async Task<Session> RenewAsync(RenewPayload payload, CancellationToken cancellationToken)
  {
    RefreshToken refreshToken;
    try
    {
      refreshToken = RefreshToken.Parse(payload.RefreshToken);
    }
    catch (Exception innerException)
    {
      throw new InvalidCredentialsException($"The refresh token '{payload.RefreshToken}' is not valid.", innerException);
    }

    SessionAggregate session = await _sessionRepository.LoadAsync(refreshToken.Id, cancellationToken)
      ?? throw new InvalidCredentialsException($"The session '{refreshToken.Id}' could not be found.");

    session.Renew(refreshToken.Secret);

    await _sessionRepository.SaveAsync(session, cancellationToken);

    Session result = await _sessionQuerier.ReadAsync(session, cancellationToken);
    if (session.Secret != null)
    {
      result.RefreshToken = new RefreshToken(session).ToString();
    }

    return result;
  }

  public virtual async Task<Session> SignInAsync(SignInPayload payload, CancellationToken cancellationToken)
  {
    UserSettings userSettings = _userSettings.Value;

    UserAggregate? user = await _userRepository.LoadAsync(payload.TenantId, payload.UniqueName, cancellationToken);
    if (user == null && userSettings.RequireUniqueEmail)
    {
      EmailAddress email = new(payload.UniqueName);
      IEnumerable<UserAggregate> users = await _userRepository.LoadAsync(payload.TenantId, email, cancellationToken);
      if (users.Count() == 1)
      {
        user = users.Single();
      }
    }

    if (user == null)
    {
      StringBuilder message = new();
      message.AppendLine("The specified user could not be found.");
      message.Append("TenantId: ").AppendLine(payload.TenantId);
      message.Append("UniqueName: ").AppendLine(payload.UniqueName);
      throw new InvalidCredentialsException(message.ToString());
    }

    return await SignInAsync(user, payload.Password, payload.IsPersistent, cancellationToken);
  }

  public virtual async Task<Session?> SignOutAsync(string id, CancellationToken cancellationToken)
  {
    AggregateId sessionId = id.GetAggregateId(nameof(id));
    SessionAggregate? session = await _sessionRepository.LoadAsync(sessionId, cancellationToken);
    if (session == null)
    {
      return null;
    }

    session.SignOut();

    await _sessionRepository.SaveAsync(session, cancellationToken);

    return await _sessionQuerier.ReadAsync(session, cancellationToken);
  }

  private async Task<Session> SignInAsync(UserAggregate user, string? password, bool isPersistent, CancellationToken cancellationToken)
  {
    UserSettings userSettings = _userSettings.Value;

    SessionAggregate session = user.SignIn(userSettings, password, isPersistent);

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
