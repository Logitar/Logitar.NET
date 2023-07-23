﻿using Logitar.EventSourcing;
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

  public async Task<User?> UpdateAsync(string id, UpdateUserPayload payload, CancellationToken cancellationToken)
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

    if (payload.FirstName != null)
    {
      user.FirstName = payload.FirstName.Value;
    }
    if (payload.LastName != null)
    {
      user.LastName = payload.LastName.Value;
    }

    if (payload.Locale != null)
    {
      user.Locale = payload.Locale.Value?.GetCultureInfo(nameof(payload.Locale));
    }

    await _userRepository.SaveAsync(user, cancellationToken);

    return await _userQuerier.ReadAsync(user, cancellationToken);
  }
}
