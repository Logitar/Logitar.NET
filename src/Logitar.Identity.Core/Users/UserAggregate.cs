using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Core.Roles;
using Logitar.Identity.Core.Settings;
using Logitar.Identity.Core.Users.Events;
using Logitar.Identity.Core.Users.Validators;
using Logitar.Identity.Core.Validators;
using Logitar.Security.Cryptography;

namespace Logitar.Identity.Core.Users;

/// <summary>
/// TODO
/// </summary>
public class UserAggregate : AggregateRoot
{
  /// <summary>
  /// The custom attributes of the user.
  /// </summary>
  private readonly Dictionary<string, string> _customAttributes = new();
  /// <summary>
  /// The external identifiers of the user.
  /// </summary>
  private readonly Dictionary<string, string> _externalIdentifiers = new();
  /// <summary>
  /// The roles of the user.
  /// </summary>
  private readonly HashSet<AggregateId> _roles = new();

  /// <summary>
  /// The password of the user.
  /// </summary>
  private Pbkdf2? _password = null;

  /// <summary>
  /// The first name of the user.
  /// </summary>
  private string? _firstName = null;
  /// <summary>
  /// The middle name of the user.
  /// </summary>
  private string? _middleName = null;
  /// <summary>
  /// The last name of the user.
  /// </summary>
  private string? _lastName = null;
  /// <summary>
  /// The nickname of the user.
  /// </summary>
  private string? _nickname = null;

  /// <summary>
  /// The birthdate of the user.
  /// </summary>
  private DateTime? _birthdate = null;
  /// <summary>
  /// The gender of the user.
  /// </summary>
  private Gender? _gender = null;
  /// <summary>
  /// The locale of the user.
  /// </summary>
  private CultureInfo? _locale = null;
  /// <summary>
  /// The time zone of the user.
  /// </summary>
  private TimeZone? _timeZone = null;

  /// <summary>
  /// The URL to the picture of the user.
  /// </summary>
  private Uri? _picture = null;
  /// <summary>
  /// The URL to the profile of the user.
  /// </summary>
  private Uri? _profile = null;
  /// <summary>
  /// The URL to the website of the user.
  /// </summary>
  private Uri? _website = null;

  /// <summary>
  /// Initializes a new instance of the <see cref="UserAggregate"/> class.
  /// </summary>
  /// <param name="id">The identifier of the user.</param>
  public UserAggregate(AggregateId id) : base(id)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="UserAggregate"/> class.
  /// </summary>
  /// <param name="uniqueNameSettings">The settings used to validate the unique name.</param>
  /// <param name="uniqueName">The unique name of the user.</param>
  /// <param name="tenantId">The identifier of the tenant in which the user belongs.</param>
  /// <param name="id">The identifier of the user.</param>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public UserAggregate(IUniqueNameSettings uniqueNameSettings, string uniqueName,
    string? tenantId = null, AggregateId? id = null) : base(id)
  {
    UserCreatedEvent e = new()
    {
      TenantId = tenantId?.CleanTrim(),
      UniqueName = uniqueName.Trim()
    };

    UniqueNameValidator uniqueNameValidator = new(uniqueNameSettings);
    new UserCreatedValidator(uniqueNameValidator).ValidateAndThrow(e);

    ApplyChange(e);
  }
  /// <summary>
  /// Applies the specified event to the aggregate.
  /// </summary>
  /// <param name="e">The event to apply.</param>
  protected virtual void Apply(UserCreatedEvent e)
  {
    TenantId = e.TenantId;

    UniqueName = e.UniqueName;
  }

  /// <summary>
  /// Gets or sets the identifier of the tenant in which the user belongs.
  /// </summary>
  public string? TenantId { get; private set; }

  /// <summary>
  /// Gets or sets the unique name of the user.
  /// </summary>
  public string UniqueName { get; private set; } = string.Empty;
  /// <summary>
  /// Gets a value indicating whether or not the user has a password.
  /// </summary>
  public bool HasPassword => _password != null;
  /// <summary>
  /// Gets or sets a value indicating whether or not the user is disabled.
  /// </summary>
  public bool IsDisabled { get; private set; }

  /// <summary>
  /// Gets or sets the first name of the user.
  /// </summary>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public string? FirstName
  {
    get => _firstName;
    set
    {
      value = value?.CleanTrim();
      if (value != null)
      {
        new PersonNameValidator(nameof(FirstName)).ValidateAndThrow(value);
      }

      if (value != _firstName)
      {
        UserModifiedEvent e = GetLatestModifiedEvent();
        e.FirstName = new Modification<string>(value);
        e.FullName = new Modification<string>(BuildFullName(value, _middleName, _lastName));
        Apply(e);
      }
    }
  }
  /// <summary>
  /// Gets or sets the middle name of the user.
  /// </summary>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public string? MiddleName
  {
    get => _middleName;
    set
    {
      value = value?.CleanTrim();
      if (value != null)
      {
        new PersonNameValidator(nameof(MiddleName)).ValidateAndThrow(value);
      }

      if (value != _middleName)
      {
        UserModifiedEvent e = GetLatestModifiedEvent();
        e.MiddleName = new Modification<string>(value);
        e.FullName = new Modification<string>(BuildFullName(_firstName, value, _lastName));
        Apply(e);
      }
    }
  }
  /// <summary>
  /// Gets or sets the last name of the user.
  /// </summary>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public string? LastName
  {
    get => _lastName;
    set
    {
      value = value?.CleanTrim();
      if (value != null)
      {
        new PersonNameValidator(nameof(LastName)).ValidateAndThrow(value);
      }

      if (value != _lastName)
      {
        UserModifiedEvent e = GetLatestModifiedEvent();
        e.LastName = new Modification<string>(value);
        e.FullName = new Modification<string>(BuildFullName(_firstName, _middleName, value));
        Apply(e);
      }
    }
  }
  /// <summary>
  /// Gets the full name of the user.
  /// </summary>
  public string? FullName { get; private set; }
  /// <summary>
  /// Gets or sets the nickname of the user.
  /// </summary>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public string? Nickname
  {
    get => _nickname;
    set
    {
      value = value?.CleanTrim();
      if (value != null)
      {
        new PersonNameValidator(nameof(Nickname)).ValidateAndThrow(value);
      }

      if (value != _nickname)
      {
        UserModifiedEvent e = GetLatestModifiedEvent();
        e.Nickname = new Modification<string>(value);
        Apply(e);
      }
    }
  }

  /// <summary>
  /// Gets or sets the birthdate of the user.
  /// </summary>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public DateTime? Birthdate
  {
    get => _birthdate;
    set
    {
      if (value.HasValue)
      {
        new BirthdateValidator(nameof(Birthdate)).ValidateAndThrow(value.Value);
      }

      if (value != _birthdate)
      {
        UserModifiedEvent e = GetLatestModifiedEvent();
        e.Birthdate = new Modification<DateTime?>(value);
        Apply(e);
      }
    }
  }
  /// <summary>
  /// Gets or sets the gender of the user.
  /// </summary>
  public Gender? Gender
  {
    get => _gender;
    set
    {
      if (value != _gender)
      {
        UserModifiedEvent e = GetLatestModifiedEvent();
        e.Gender = new Modification<Gender>(value);
        Apply(e);
      }
    }
  }
  /// <summary>
  /// Gets or sets the locale of the user.
  /// </summary>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public CultureInfo? Locale
  {
    get => _locale;
    set
    {
      if (value != null)
      {
        new LocaleValidator(nameof(Locale)).ValidateAndThrow(value);
      }

      if (value?.LCID != _locale?.LCID)
      {
        UserModifiedEvent e = GetLatestModifiedEvent();
        e.Locale = new Modification<CultureInfo>(value);
        Apply(e);
      }
    }
  }
  /// <summary>
  /// Gets or sets the time zone of the user.
  /// </summary>
  public TimeZone? TimeZone
  {
    get => _timeZone;
    set
    {
      if (value != _timeZone)
      {
        UserModifiedEvent e = GetLatestModifiedEvent();
        e.TimeZone = new Modification<TimeZone>(value);
        Apply(e);
      }
    }
  }

  /// <summary>
  /// Gets or sets the URL to the picture of the user.
  /// </summary>
  public Uri? Picture
  {
    get => _picture;
    set
    {
      if (value != _picture)
      {
        UserModifiedEvent e = GetLatestModifiedEvent();
        e.Picture = new Modification<Uri>(value);
        Apply(e);
      }
    }
  }
  /// <summary>
  /// Gets or sets the URL to the profile of the user.
  /// </summary>
  public Uri? Profile
  {
    get => _profile;
    set
    {
      if (value != _profile)
      {
        UserModifiedEvent e = GetLatestModifiedEvent();
        e.Profile = new Modification<Uri>(value);
        Apply(e);
      }
    }
  }
  /// <summary>
  /// Gets or sets the URL to the website of the user.
  /// </summary>
  public Uri? Website
  {
    get => _website;
    set
    {
      if (value != _website)
      {
        UserModifiedEvent e = GetLatestModifiedEvent();
        e.Website = new Modification<Uri>(value);
        Apply(e);
      }
    }
  }

  /// <summary>
  /// Gets the custom attributes of the user.
  /// </summary>
  public IReadOnlyDictionary<string, string> CustomAttributes => _customAttributes.AsReadOnly();
  /// <summary>
  /// Gets the external identifiers of the user.
  /// </summary>
  public IReadOnlyDictionary<string, string> ExternalIdentifiers => _externalIdentifiers.AsReadOnly();
  /// <summary>
  /// Gets the roles of the user.
  /// </summary>
  public IReadOnlySet<AggregateId> Roles => _roles.ToImmutableHashSet();

  /// <summary>
  /// Adds the specified role to the user.
  /// </summary>
  /// <param name="role">The role to add.</param>
  public void AddRole(RoleAggregate role)
  {
    if (!_roles.Contains(role.Id))
    {
      SetRole(role, addOrRemove: true);
    }
  }
  /// <summary>
  /// Removes the specified role from the user.
  /// </summary>
  /// <param name="role">The role to remove.</param>
  public void RemoveRole(RoleAggregate role)
  {
    if (_roles.Contains(role.Id))
    {
      SetRole(role, addOrRemove: false);
    }
  }
  /// <summary>
  /// Sets the status of the specified role.
  /// </summary>
  /// <param name="role">The role to add or remove.</param>
  /// <param name="addOrRemove">A value indicating whether the role will be added if true or removed if false.</param>
  private void SetRole(RoleAggregate role, bool addOrRemove)
  {
    UserModifiedEvent e = GetLatestModifiedEvent();
    e.Roles[role.Id.Value] = addOrRemove;
    Apply(e);
  }

  /// <summary>
  /// Changes the password of the user.
  /// </summary>
  /// <param name="passwordSettings">The settings used to validate the password.</param>
  /// <param name="password">The new password of the user.</param>
  /// <param name="current">The current password of the user, used to validate a password change from an user.</param>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public void ChangePassword(IPasswordSettings passwordSettings, string password, string? current = null)
  {
    if (current != null && _password?.IsMatch(current) != true)
    {
      StringBuilder message = new();
      message.AppendLine("The user password was not a match.");
      message.Append("User: ").Append(this).AppendLine();
      message.Append("Password: ").Append(password).AppendLine();
      throw new InvalidCredentialsException(message.ToString());
    }

    new PasswordValidator(passwordSettings).ValidateAndThrow(password);

    ApplyChange(new UserPasswordChangedEvent
    {
      Password = new(password)
    });
  }
  /// <summary>
  /// Applies the specified event to the aggregate.
  /// </summary>
  /// <param name="e">The event to apply.</param>
  protected virtual void Apply(UserPasswordChangedEvent e) => _password = e.Password;

  /// <summary>
  /// Deletes the role.
  /// </summary>
  public void Delete() => ApplyChange(new UserDeletedEvent());

  /// <summary>
  /// Disables the user.
  /// </summary>
  public void Disable()
  {
    if (!IsDisabled)
    {
      ApplyChange(new UserStatusChangedEvent
      {
        IsDisabled = true
      });
    }
  }
  /// <summary>
  /// Enables the user.
  /// </summary>
  public void Enable()
  {
    if (IsDisabled)
    {
      ApplyChange(new UserStatusChangedEvent
      {
        IsDisabled = false
      });
    }
  }
  /// <summary>
  /// Applies the specified event to the aggregate.
  /// </summary>
  /// <param name="e">The event to apply.</param>
  protected virtual void Apply(UserStatusChangedEvent e) => IsDisabled = e.IsDisabled;

  /// <summary>
  /// Removes a custom attribute on the user.
  /// </summary>
  /// <param name="key">The key of the custom attribute.</param>
  public void RemoveCustomAttribute(string key)
  {
    key = key.Trim();
    if (_customAttributes.ContainsKey(key))
    {
      UserModifiedEvent e = GetLatestModifiedEvent();
      e.CustomAttributes[key] = null;
      Apply(e);
    }
  }
  /// <summary>
  /// Sets a custom attribute on the user.
  /// </summary>
  /// <param name="key">The key of the custom attribute.</param>
  /// <param name="value">The value of the custom attribute.</param>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public void SetCustomAttribute(string key, string value)
  {
    key = key.Trim();
    value = value.Trim();
    CustomAttributeValidator.Instance.ValidateAndThrow(key, value);

    if (!_customAttributes.TryGetValue(key, out string? existingValue) || value != existingValue)
    {
      UserModifiedEvent e = GetLatestModifiedEvent();
      e.CustomAttributes[key] = value;
      Apply(e);
    }
  }

  /// <summary>
  /// Removes an external identifier from the user.
  /// </summary>
  /// <param name="key">The key of the external identifier.</param>
  public void RemoveExternalIdentifier(string key)
  {
    key = key.Trim();
    if (_externalIdentifiers.ContainsKey(key))
    {
      ApplyChange(new UserExternalIdentifierChangedEvent
      {
        Key = key,
        Value = null
      });
    }
  }
  /// <summary>
  /// Sets an external identifier to the user.
  /// </summary>
  /// <param name="key">The key of the external identifier.</param>
  /// <param name="value">The value of the external identifier.</param>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public void SetExternalIdentifier(string key, string value)
  {
    key = key.Trim();
    value = value.Trim();
    ExternalIdentifierValidator.Instance.ValidateAndThrow(key, value);

    if (!_externalIdentifiers.TryGetValue(key, out string? existingValue) || value != existingValue)
    {
      ApplyChange(new UserExternalIdentifierChangedEvent
      {
        Key = key,
        Value = value
      });
    }
  }
  /// <summary>
  /// Applies the specified event to the aggregate.
  /// </summary>
  /// <param name="e">The event to apply.</param>
  protected virtual void Apply(UserExternalIdentifierChangedEvent e)
  {
    if (e.Value == null)
    {
      _externalIdentifiers.Remove(e.Key);
    }
    else
    {
      _externalIdentifiers[e.Key] = e.Value;
    }
  }

  /// <summary>
  /// Changes the unique name of the user.
  /// </summary>
  /// <param name="uniqueNameSettings">The settings used to validate the unique name.</param>
  /// <param name="uniqueName">The unique name of the user.</param>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public void SetUniqueName(IUniqueNameSettings uniqueNameSettings, string uniqueName)
  {
    uniqueName = uniqueName.Trim();
    new UniqueNameValidator(uniqueNameSettings, nameof(UniqueName)).ValidateAndThrow(uniqueName);

    if (uniqueName != UniqueName)
    {
      ApplyChange(new UserUniqueNameChangedEvent
      {
        UniqueName = uniqueName
      });
    }
  }
  /// <summary>
  /// Applies the specified event to the aggregate.
  /// </summary>
  /// <param name="e">The event to apply.</param>
  protected virtual void Apply(UserUniqueNameChangedEvent e) => UniqueName = e.UniqueName;

  /// <summary>
  /// Applies the specified event to the aggregate.
  /// </summary>
  /// <param name="e">The event to apply.</param>
  protected virtual void Apply(UserModifiedEvent e)
  {
    if (e.FirstName.IsModified)
    {
      _firstName = e.FirstName.Value;
    }
    if (e.MiddleName.IsModified)
    {
      _middleName = e.MiddleName.Value;
    }
    if (e.LastName.IsModified)
    {
      _lastName = e.LastName.Value;
    }
    if (e.FullName.IsModified)
    {
      FullName = e.FullName.Value;
    }
    if (e.Nickname.IsModified)
    {
      _nickname = e.Nickname.Value;
    }

    if (e.Birthdate.IsModified)
    {
      _birthdate = e.Birthdate.Value;
    }
    if (e.Gender.IsModified)
    {
      _gender = e.Gender.Value;
    }
    if (e.Locale.IsModified)
    {
      _locale = e.Locale.Value;
    }
    if (e.TimeZone.IsModified)
    {
      _timeZone = e.TimeZone.Value;
    }

    if (e.Picture.IsModified)
    {
      _picture = e.Picture.Value;
    }
    if (e.Profile.IsModified)
    {
      _profile = e.Profile.Value;
    }
    if (e.Website.IsModified)
    {
      _website = e.Website.Value;
    }

    foreach (KeyValuePair<string, string?> customAttribute in e.CustomAttributes)
    {
      if (customAttribute.Value == null)
      {
        _customAttributes.Remove(customAttribute.Key);
      }
      else
      {
        _customAttributes[customAttribute.Key] = customAttribute.Value;
      }
    }

    foreach (KeyValuePair<string, bool> role in e.Roles)
    {
      AggregateId roleId = new(role.Key);

      if (role.Value)
      {
        _roles.Add(roleId);
      }
      else
      {
        _roles.Remove(roleId);
      }
    }
  }
  /// <summary>
  /// Finds or applies the latest user modification event.
  /// </summary>
  /// <returns>The latest user modification event.</returns>
  private UserModifiedEvent GetLatestModifiedEvent()
  {
    UserModifiedEvent? e = Changes.LastOrDefault(e => e is UserModifiedEvent) as UserModifiedEvent;
    if (e == null)
    {
      e = new();
      ApplyChange(e);
    }

    return e;
  }

  /// <summary>
  /// Builds the full name of the user.
  /// </summary>
  /// <param name="names">The names of the user.</param>
  /// <returns>The full name of the user.</returns>
  private static string? BuildFullName(params string?[] names) => string.Join(' ', names
    .SelectMany(name => name?.Split() ?? Array.Empty<string>())
    .Where(name => !string.IsNullOrEmpty(name)));

  /// <summary>
  /// Returns a string representation of the user aggregate.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => string.Join(" | ", FullName ?? UniqueName, base.ToString());
}
