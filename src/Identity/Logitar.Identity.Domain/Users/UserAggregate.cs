using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users.Events;
using Logitar.Identity.Domain.Users.Validators;
using Logitar.Identity.Domain.Validators;

namespace Logitar.Identity.Domain.Users;

public class UserAggregate : AggregateRoot
{
  private readonly Dictionary<string, string> _customAttributes = new();
  private readonly HashSet<AggregateId> _roles = new();
  private readonly CustomAttributeValidator _customAttributeValidator = new();

  private Password? _password = null;

  private PostalAddress? _address = null;
  private EmailAddress? _email = null;
  private PhoneNumber? _phone = null;

  private string? _firstName = null;
  private string? _middleName = null;
  private string? _lastName = null;
  private string? _nickname = null;

  private DateTime? _birthdate = null;
  private Gender? _gender = null;
  private CultureInfo? _locale = null;
  private TimeZoneEntry? _timeZone = null;

  private Uri? _picture = null;
  private Uri? _profile = null;
  private Uri? _website = null;

  public UserAggregate(AggregateId id) : base(id)
  {
  }

  public UserAggregate(IUniqueNameSettings uniqueNameSettings, string uniqueName, string? tenantId = null) : base()
  {
    UserCreatedEvent created = new()
    {
      UniqueName = uniqueName.Trim(),
      TenantId = tenantId?.CleanTrim()
    };
    new UserCreatedValidator(uniqueNameSettings).ValidateAndThrow(created);

    ApplyChange(created);
  }
  protected virtual void Apply(UserCreatedEvent created)
  {
    TenantId = created.TenantId;

    UniqueName = created.UniqueName;
  }

  public string? TenantId { get; private set; }

  public string UniqueName { get; private set; } = string.Empty;
  public bool HasPassword => _password != null;
  public bool IsDisabled { get; private set; }

  public PostalAddress? Address
  {
    get => _address;
    set
    {
      if (value != _address)
      {
        UserUpdatedEvent updated = GetLatestEvent<UserUpdatedEvent>();
        updated.Address = new MayBe<PostalAddress>(value);
        Apply(updated);
      }
    }
  }
  public EmailAddress? Email
  {
    get => _email;
    set
    {
      if (value != _email)
      {
        UserUpdatedEvent updated = GetLatestEvent<UserUpdatedEvent>();
        updated.Email = new MayBe<EmailAddress>(value);
        Apply(updated);
      }
    }
  }
  public PhoneNumber? Phone
  {
    get => _phone;
    set
    {
      if (value != _phone)
      {
        UserUpdatedEvent updated = GetLatestEvent<UserUpdatedEvent>();
        updated.Phone = new MayBe<PhoneNumber>(value);
        Apply(updated);
      }
    }
  }
  public bool IsConfirmed => Address?.IsVerified == true || Email?.IsVerified == true || Phone?.IsVerified == true;

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
        UserUpdatedEvent updated = GetLatestEvent<UserUpdatedEvent>();
        updated.FirstName = new MayBe<string>(value);
        updated.FullName = new MayBe<string>(PersonHelper.BuildFullName(value, _middleName, _lastName));
        Apply(updated);
      }
    }
  }
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
        UserUpdatedEvent updated = GetLatestEvent<UserUpdatedEvent>();
        updated.MiddleName = new MayBe<string>(value);
        updated.FullName = new MayBe<string>(PersonHelper.BuildFullName(_firstName, value, _lastName));
        Apply(updated);
      }
    }
  }
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
        UserUpdatedEvent updated = GetLatestEvent<UserUpdatedEvent>();
        updated.LastName = new MayBe<string>(value);
        updated.FullName = new MayBe<string>(PersonHelper.BuildFullName(_firstName, _middleName, value));
        Apply(updated);
      }
    }
  }
  public string? FullName { get; private set; }
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
        UserUpdatedEvent updated = GetLatestEvent<UserUpdatedEvent>();
        updated.Nickname = new MayBe<string>(value);
        Apply(updated);
      }
    }
  }

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
        UserUpdatedEvent updated = GetLatestEvent<UserUpdatedEvent>();
        updated.Birthdate = new MayBe<DateTime?>(value);
        Apply(updated);
      }
    }
  }
  public Gender? Gender
  {
    get => _gender;
    set
    {
      if (value != _gender)
      {
        UserUpdatedEvent updated = GetLatestEvent<UserUpdatedEvent>();
        updated.Gender = new MayBe<Gender>(value);
        Apply(updated);
      }
    }
  }
  public CultureInfo? Locale
  {
    get => _locale;
    set
    {
      if (value != null)
      {
        new LocaleValidator(nameof(Locale)).ValidateAndThrow(value);
      }

      if (value != _locale)
      {
        UserUpdatedEvent updated = GetLatestEvent<UserUpdatedEvent>();
        updated.Locale = new MayBe<CultureInfo>(value);
        Apply(updated);
      }
    }
  }
  public TimeZoneEntry? TimeZone
  {
    get => _timeZone;
    set
    {
      if (value != _timeZone)
      {
        UserUpdatedEvent updated = GetLatestEvent<UserUpdatedEvent>();
        updated.TimeZone = new MayBe<TimeZoneEntry>(value);
        Apply(updated);
      }
    }
  }

  public Uri? Picture
  {
    get => _picture;
    set
    {
      if (value != _picture)
      {
        UserUpdatedEvent updated = GetLatestEvent<UserUpdatedEvent>();
        updated.Picture = new MayBe<Uri>(value);
        Apply(updated);
      }
    }
  }
  public Uri? Profile
  {
    get => _profile;
    set
    {
      if (value != _profile)
      {
        UserUpdatedEvent updated = GetLatestEvent<UserUpdatedEvent>();
        updated.Picture = new MayBe<Uri>(value);
        Apply(updated);
      }
    }
  }
  public Uri? Website
  {
    get => _website;
    set
    {
      if (value != _website)
      {
        UserUpdatedEvent updated = GetLatestEvent<UserUpdatedEvent>();
        updated.Website = new MayBe<Uri>(value);
        Apply(updated);
      }
    }
  }

  public IReadOnlyDictionary<string, string> CustomAttributes => _customAttributes;

  public IImmutableSet<AggregateId> Roles => ImmutableHashSet.Create(_roles.ToArray());

  public void AddRole(RoleAggregate role)
  {
    if (!_roles.Contains(role.Id))
    {
      UserUpdatedEvent updated = GetLatestEvent<UserUpdatedEvent>();
      updated.Roles[role.Id.Value] = Action.Add;
      Apply(updated);
    }
  }

  public void Delete() => ApplyChange(new UserDeletedEvent());

  public void Disable()
  {
    if (!IsDisabled)
    {
      ApplyChange(new UserDisabledEvent());
    }
  }
  protected virtual void Apply(UserDisabledEvent _) => IsDisabled = true;

  public void Enable()
  {
    if (IsDisabled)
    {
      ApplyChange(new UserEnabledEvent());
    }
  }
  protected virtual void Apply(UserEnabledEvent _) => IsDisabled = false;

  public void RemoveCustomAttribute(string key)
  {
    key = key.Trim();
    if (_customAttributes.ContainsKey(key))
    {
      UserUpdatedEvent updated = GetLatestEvent<UserUpdatedEvent>();
      updated.CustomAttributes[key] = null;
      Apply(updated);
    }
  }

  public void RemoveRole(RoleAggregate role)
  {
    if (_roles.Contains(role.Id))
    {
      UserUpdatedEvent updated = GetLatestEvent<UserUpdatedEvent>();
      updated.Roles[role.Id.Value] = Action.Remove;
      Apply(updated);
    }
  }

  public void SetCustomAttribute(string key, string value)
  {
    key = key.Trim();
    value = value.Trim();
    _customAttributeValidator.ValidateAndThrow(key, value);

    if (!_customAttributes.TryGetValue(key, out string? existingValue) || value != existingValue)
    {
      UserUpdatedEvent updated = GetLatestEvent<UserUpdatedEvent>();
      updated.CustomAttributes[key] = value;
      Apply(updated);
    }
  }

  public void SetPassword(Password password)
  {
    UserUpdatedEvent updated = GetLatestEvent<UserUpdatedEvent>();
    updated.Password = password;
    Apply(updated);
  }

  public void SetUniqueName(IUniqueNameSettings uniqueNameSettings, string uniqueName)
  {
    uniqueName = uniqueName.Trim();
    new UniqueNameValidator(uniqueNameSettings, nameof(UniqueName)).ValidateAndThrow(uniqueName);

    UserUpdatedEvent updated = GetLatestEvent<UserUpdatedEvent>();
    updated.UniqueName = uniqueName;
    Apply(updated);
  }

  protected virtual void Apply(UserUpdatedEvent updated)
  {
    if (updated.UniqueName != null)
    {
      UniqueName = updated.UniqueName;
    }
    if (updated.Password != null)
    {
      _password = updated.Password;
    }

    if (updated.Address != null)
    {
      _address = updated.Address.Value;
    }
    if (updated.Email != null)
    {
      _email = updated.Email.Value;
    }
    if (updated.Phone != null)
    {
      _phone = updated.Phone.Value;
    }

    if (updated.FirstName != null)
    {
      _firstName = updated.FirstName.Value;
    }
    if (updated.MiddleName != null)
    {
      _middleName = updated.MiddleName.Value;
    }
    if (updated.LastName != null)
    {
      _lastName = updated.LastName.Value;
    }
    if (updated.FullName != null)
    {
      FullName = updated.FullName.Value;
    }
    if (updated.Nickname != null)
    {
      _nickname = updated.Nickname.Value;
    }

    if (updated.Birthdate != null)
    {
      _birthdate = updated.Birthdate.Value;
    }
    if (updated.Gender != null)
    {
      _gender = updated.Gender.Value;
    }
    if (updated.Locale != null)
    {
      _locale = updated.Locale.Value;
    }
    if (updated.TimeZone != null)
    {
      _timeZone = updated.TimeZone.Value;
    }

    if (updated.Picture != null)
    {
      _picture = updated.Picture.Value;
    }
    if (updated.Profile != null)
    {
      _profile = updated.Profile.Value;
    }
    if (updated.Website != null)
    {
      _website = updated.Website.Value;
    }

    foreach (KeyValuePair<string, string?> customAttributes in updated.CustomAttributes)
    {
      if (customAttributes.Value == null)
      {
        _customAttributes.Remove(customAttributes.Key);
      }
      else
      {
        _customAttributes[customAttributes.Key] = customAttributes.Value;
      }
    }
    foreach (KeyValuePair<string, Action> role in updated.Roles)
    {
      switch (role.Value)
      {
        case Action.Add:
          _roles.Add(new AggregateId(role.Key));
          break;
        case Action.Remove:
          _roles.Remove(new AggregateId(role.Key));
          break;
      }
    }
  }
  protected virtual T GetLatestEvent<T>() where T : DomainEvent, new()
  {
    T? change = Changes.LastOrDefault(change => change is T) as T;
    if (change == null)
    {
      change = new();
      ApplyChange(change);
    }

    return change;
  }

  public override string ToString() => $"{FullName ?? UniqueName} | {base.ToString()}";
}
