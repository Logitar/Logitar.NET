using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Core.Users.Payloads;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.Core.Users;

public class UserService : IUserService
{
  private readonly IUserQuerier _userQuerier;
  private readonly IUserRepository _userRepository;
  private readonly IOptions<UserSettings> _userSettings;

  public UserService(IUserQuerier userQuerier, IUserRepository userRepository,
    IOptions<UserSettings> userSettings)
  {
    _userQuerier = userQuerier;
    _userRepository = userRepository;
    _userSettings = userSettings;
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

    user.FirstName = payload.FirstName;
    user.LastName = payload.LastName;

    user.Locale = payload.Locale?.GetCultureInfo(nameof(payload.Locale));

    await _userRepository.SaveAsync(user, cancellationToken);

    return await _userQuerier.ReadAsync(user, cancellationToken);
  }
}
