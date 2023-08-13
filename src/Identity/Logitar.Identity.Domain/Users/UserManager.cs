using Logitar.EventSourcing;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users.Events;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.Domain.Users;

public class UserManager : IUserManager
{
  private readonly IAggregateRepository _aggregateRepository;
  private readonly ISessionRepository _sessionRepository;
  private readonly IUserRepository _userRepository;
  private readonly IOptions<UserSettings> _userSettings;

  public UserManager(IAggregateRepository aggregateRepository, ISessionRepository sessionRepository,
    IUserRepository userRepository, IOptions<UserSettings> userSettings)
  {
    _aggregateRepository = aggregateRepository;
    _sessionRepository = sessionRepository;
    _userRepository = userRepository;
    _userSettings = userSettings;
  }

  public async Task DeleteAsync(UserAggregate user, CancellationToken cancellationToken)
  {
    IEnumerable<SessionAggregate> sessions = await _sessionRepository.LoadAsync(user, cancellationToken);
    foreach (SessionAggregate session in sessions)
    {
      session.Delete();
    }
    await _aggregateRepository.SaveAsync(sessions, cancellationToken);

    user.Delete();
    await _aggregateRepository.SaveAsync(user, cancellationToken);
  }

  public async Task SaveAsync(UserAggregate user, CancellationToken cancellationToken)
  {
    UserSettings userSettings = _userSettings.Value;

    bool hasUniqueNameChanged = false;
    bool hasEmailAddressChanged = false;
    foreach (DomainEvent change in user.Changes)
    {
      if (change is UserCreatedEvent)
      {
        hasUniqueNameChanged = true;
      }
      else if (change is UserUpdatedEvent updated)
      {
        if (updated.UniqueName != null)
        {
          hasUniqueNameChanged = true;
        }
        if (updated.Email?.Value != null)
        {
          hasEmailAddressChanged = true;
        }
      }

      if (hasUniqueNameChanged && hasEmailAddressChanged)
      {
        break;
      }
    }
    if (hasUniqueNameChanged)
    {
      UserAggregate? other = await _userRepository.LoadAsync(user.TenantId, user.UniqueName, cancellationToken);
      if (other?.Equals(user) == false)
      {
        throw new UniqueNameAlreadyUsedException<UserAggregate>(user.TenantId, user.UniqueName, nameof(user.UniqueName));
      }
    }
    if (userSettings.RequireUniqueEmail && hasEmailAddressChanged && user.Email != null)
    {
      IEnumerable<UserAggregate> users = await _userRepository.LoadAsync(user.TenantId, user.Email, cancellationToken);
      if (users.Any(u => !u.Equals(user)))
      {
        throw new EmailAddressAlreadyUsedException(user.TenantId, user.Email, nameof(user.Email));
      }
    }

    await _aggregateRepository.SaveAsync(user, cancellationToken);
  }
}
