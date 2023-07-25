using Logitar.EventSourcing;
using Logitar.Identity.Core.Sessions;
using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Core.Users.Payloads;
using Logitar.Identity.Domain;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.Core.Users;

public class UserService : IUserService
{
  private readonly ISessionRepository _sessionRepository;
  private readonly IUserQuerier _userQuerier;
  private readonly IUserRepository _userRepository;
  private readonly IOptions<UserSettings> _userSettings;

  public UserService(ISessionRepository sessionRepository, IUserQuerier userQuerier,
    IUserRepository userRepository, IOptions<UserSettings> userSettings)
  {
    _sessionRepository = sessionRepository;
    _userQuerier = userQuerier;
    _userRepository = userRepository;
    _userSettings = userSettings;
  }

  public virtual async Task<User> AuthenticateAsync(AuthenticateUserPayload payload, CancellationToken cancellationToken)
  {
    UserAggregate? user = await _userRepository.LoadAsync(payload.TenantId, payload.UniqueName, cancellationToken);
    if (user == null)
    {
      StringBuilder message = new();
      message.AppendLine("The specified user could not be found.");
      message.Append("TenantId: ").AppendLine(payload.TenantId);
      message.Append("UniqueName: ").AppendLine(payload.UniqueName);
      throw new InvalidCredentialsException(message.ToString());
    }

    user.Authenticate(_userSettings.Value, payload.Password);

    await _userRepository.SaveAsync(user, cancellationToken);

    return await _userQuerier.ReadAsync(user, cancellationToken);
  }

  public virtual async Task<User?> ChangePasswordAsync(string id, ChangePasswordPayload payload, CancellationToken cancellationToken)
  {
    AggregateId userId = id.GetAggregateId(nameof(id));
    UserAggregate? user = await _userRepository.LoadAsync(userId, cancellationToken);
    if (user == null)
    {
      return null;
    }

    UserSettings userSettings = _userSettings.Value;

    user.ChangePassword(userSettings.PasswordSettings, payload.Password, payload.Current);

    await _userRepository.SaveAsync(user, cancellationToken);

    return await _userQuerier.ReadAsync(user, cancellationToken);
  }

  public virtual async Task<User> CreateAsync(CreateUserPayload payload, CancellationToken cancellationToken)
  {
    if (await _userRepository.LoadAsync(payload.TenantId, payload.UniqueName, cancellationToken) != null)
    {
      throw new UniqueNameAlreadyUsedException<UserAggregate>(payload.TenantId, payload.UniqueName, nameof(payload.UniqueName));
    }

    UserSettings userSettings = _userSettings.Value;

    UserAggregate user = new(userSettings.UniqueNameSettings, payload.UniqueName, payload.TenantId);

    if (payload.Password != null)
    {
      user.SetPassword(userSettings.PasswordSettings, payload.Password);
    }

    if (payload.IsDisabled)
    {
      user.Disable();
    }

    if (payload.Email != null)
    {
      EmailAddress email = new(payload.Email.Address, payload.Email.IsVerified);
      if (userSettings.RequireUniqueEmail
        && (await _userRepository.LoadAsync(user.TenantId, email, cancellationToken)).Any())
      {
        throw new EmailAddressAlreadyUsedException(user.TenantId, email, nameof(payload.Email));
      }

      user.Email = email;
    }
    if (payload.Phone != null)
    {
      PhoneNumber phone = new(payload.Phone.Number, payload.Phone.CountryCode,
        payload.Phone.Extension, payload.Phone.IsVerified);
      user.Phone = phone;
    }

    user.FirstName = payload.FirstName;
    user.MiddleName = payload.MiddleName;
    user.LastName = payload.LastName;
    user.Nickname = payload.Nickname;

    user.Birthdate = payload.Birthdate;
    user.Gender = payload.Gender?.GetGender(nameof(payload.Gender));
    user.Locale = payload.Locale?.GetCultureInfo(nameof(payload.Locale));
    user.TimeZone = payload.TimeZone?.GetTimeZone(nameof(payload.TimeZone));

    user.Picture = payload.Picture?.GetUri(nameof(payload.Picture));
    user.Profile = payload.Profile?.GetUri(nameof(payload.Profile));
    user.Website = payload.Website?.GetUri(nameof(payload.Website));

    await _userRepository.SaveAsync(user, cancellationToken);

    return await _userQuerier.ReadAsync(user, cancellationToken);
  }

  public virtual async Task<User?> DeleteAsync(string id, CancellationToken cancellationToken)
  {
    AggregateId userId = id.GetAggregateId(nameof(id));
    UserAggregate? user = await _userRepository.LoadAsync(userId, cancellationToken);
    if (user == null)
    {
      return null;
    }
    User result = await _userQuerier.ReadAsync(user, cancellationToken);

    user.Delete();

    // TODO(fpion): delete user sessions

    await _userRepository.SaveAsync(user, cancellationToken);

    return result;
  }

  public virtual async Task<User?> ReadAsync(string? id, string? tenantId, string? uniqueName, CancellationToken cancellationToken)
  {
    Dictionary<string, User> users = new(capacity: 2);

    if (id != null)
    {
      User? user = await _userQuerier.ReadAsync(id, cancellationToken);
      if (user != null)
      {
        users[user.Id] = user;
      }
    }

    if (uniqueName != null)
    {
      User? user = await _userQuerier.ReadAsync(tenantId, uniqueName, cancellationToken);
      if (user != null)
      {
        users[user.Id] = user;
      }
      else
      {
        UserSettings userSettings = _userSettings.Value;
        if (userSettings.RequireUniqueEmail)
        {
          EmailAddress email = new(uniqueName);
          IEnumerable<User> foundUsers = await _userQuerier.ReadAsync(tenantId, email, cancellationToken);
          if (foundUsers.Count() == 1)
          {
            user = foundUsers.Single();
            users[user.Id] = user;
          }
        }
      }
    }

    if (users.Count > 1)
    {
      throw new TooManyResultsException<User>(expected: 1, actual: users.Count);
    }

    return users.Values.SingleOrDefault();
  }

  public virtual async Task<User?> SignOutAsync(string id, CancellationToken cancellationToken)
  {
    AggregateId userId = id.GetAggregateId(nameof(id));
    UserAggregate? user = await _userRepository.LoadAsync(userId, cancellationToken);
    if (user == null)
    {
      return null;
    }

    IEnumerable<SessionAggregate> sessions = await _sessionRepository.LoadActiveAsync(user, cancellationToken);
    foreach (SessionAggregate session in sessions)
    {
      session.SignOut();
    }

    await _sessionRepository.SaveAsync(sessions, cancellationToken);

    return await _userQuerier.ReadAsync(user, cancellationToken);
  }

  public virtual async Task<User?> UpdateAsync(string id, UpdateUserPayload payload, CancellationToken cancellationToken)
  {
    AggregateId userId = id.GetAggregateId(nameof(id));
    UserAggregate? user = await _userRepository.LoadAsync(userId, cancellationToken);
    if (user == null)
    {
      return null;
    }

    UserSettings userSettings = _userSettings.Value;

    if (payload.UniqueName != null)
    {
      UserAggregate? other = await _userRepository.LoadAsync(user.TenantId, payload.UniqueName, cancellationToken);
      if (other?.Equals(user) == false)
      {
        throw new UniqueNameAlreadyUsedException<UserAggregate>(user.TenantId, payload.UniqueName, nameof(payload.UniqueName));
      }

      user.SetUniqueName(userSettings.UniqueNameSettings, payload.UniqueName);
    }
    if (payload.Password != null)
    {
      user.SetPassword(userSettings.PasswordSettings, payload.Password);
    }

    if (payload.IsDisabled != null)
    {
      if (payload.IsDisabled.Value)
      {
        user.Disable();
      }
      else
      {
        user.Enable();
      }
    }

    if (payload.Email != null)
    {
      bool isVerified = payload.Email.Value?.IsVerified ?? user.Email?.IsVerified ?? false;
      EmailAddress? email = payload.Email.Value == null ? null
        : new(payload.Email.Value.Address, isVerified);
      if (email != null)
      {
        IEnumerable<UserAggregate> users = await _userRepository.LoadAsync(user.TenantId, email, cancellationToken);
        if (users.Any(u => !u.Equals(user)))
        {
          throw new EmailAddressAlreadyUsedException(user.TenantId, email, nameof(payload.Email));
        }
      }

      user.Email = email;
    }
    if (payload.Phone != null)
    {
      bool isVerified = payload.Phone.Value?.IsVerified ?? user.Phone?.IsVerified ?? false;
      PhoneNumber? phone = payload.Phone.Value == null ? null
        : new(payload.Phone.Value.Number, payload.Phone.Value.CountryCode,
          payload.Phone.Value.Extension, isVerified);
      user.Phone = phone;
    }

    if (payload.FirstName != null)
    {
      user.FirstName = payload.FirstName.Value;
    }
    if (payload.MiddleName != null)
    {
      user.MiddleName = payload.MiddleName.Value;
    }
    if (payload.LastName != null)
    {
      user.LastName = payload.LastName.Value;
    }
    if (payload.Nickname != null)
    {
      user.Nickname = payload.Nickname.Value;
    }

    if (payload.Birthdate != null)
    {
      user.Birthdate = payload.Birthdate.Value;
    }
    if (payload.Gender != null)
    {
      user.Gender = payload.Gender.Value?.GetGender(nameof(payload.Gender));
    }
    if (payload.Locale != null)
    {
      user.Locale = payload.Locale.Value?.GetCultureInfo(nameof(payload.Locale));
    }
    if (payload.TimeZone != null)
    {
      user.TimeZone = payload.TimeZone.Value?.GetTimeZone(nameof(payload.TimeZone));
    }

    if (payload.Picture != null)
    {
      user.Picture = payload.Picture.Value?.GetUri(nameof(payload.Picture));
    }
    if (payload.Profile != null)
    {
      user.Profile = payload.Profile.Value?.GetUri(nameof(payload.Profile));
    }
    if (payload.Website != null)
    {
      user.Website = payload.Website.Value?.GetUri(nameof(payload.Website));
    }

    await _userRepository.SaveAsync(user, cancellationToken);

    return await _userQuerier.ReadAsync(user, cancellationToken);
  }
}
