using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users.Events;
using Logitar.Identity.Domain.Users.Validators;
using Logitar.Identity.Domain.Validators;
using Logitar.Security;

namespace Logitar.Identity.Domain.Users;

public class UserAggregate : AggregateRoot
{
  private Pbkdf2? _password = null;

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
      TenantId = tenantId?.CleanTrim(),
      UniqueName = uniqueName.Trim()
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

  public EmailAddress? Email
  {
    get => _email;
    set
    {
      if (value != null)
      {
        new EmailAddressValidator().ValidateAndThrow(value);
      }

      if (value != _email)
      {
        UserUpdatedEvent updated = GetLatestUpdatedEvent();
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
      if (value != null)
      {
        new PhoneNumberValidator().ValidateAndThrow(value);
      }

      if (value != _phone)
      {
        UserUpdatedEvent updated = GetLatestUpdatedEvent();
        updated.Phone = new MayBe<PhoneNumber>(value);
        Apply(updated);
      }
    }
  }
  public bool IsConfirmed => Email?.IsVerified == true || Phone?.IsVerified == true;

  public DateTime? AuthenticatedOn { get; private set; }

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
        UserUpdatedEvent updated = GetLatestUpdatedEvent();
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
        UserUpdatedEvent updated = GetLatestUpdatedEvent();
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
        UserUpdatedEvent updated = GetLatestUpdatedEvent();
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
        UserUpdatedEvent updated = GetLatestUpdatedEvent();
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
        UserUpdatedEvent updated = GetLatestUpdatedEvent();
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
        UserUpdatedEvent updated = GetLatestUpdatedEvent();
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
        UserUpdatedEvent updated = GetLatestUpdatedEvent();
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
        UserUpdatedEvent updated = GetLatestUpdatedEvent();
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
        UserUpdatedEvent updated = GetLatestUpdatedEvent();
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
        UserUpdatedEvent updated = GetLatestUpdatedEvent();
        updated.Profile = new MayBe<Uri>(value);
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
        UserUpdatedEvent updated = GetLatestUpdatedEvent();
        updated.Website = new MayBe<Uri>(value);
        Apply(updated);
      }
    }
  }

  public void Authenticate(IUserSettings userSettings, string password)
  {
    CheckStatus(userSettings);
    CheckPassword(password);

    ApplyChange(new UserAuthenticatedEvent(), actorId: Id.Value);
  }
  protected virtual void Apply(UserAuthenticatedEvent authenticated) => AuthenticatedOn = authenticated.OccurredOn;

  public void ChangePassword(IPasswordSettings passwordSettings, string password, string current)
  {
    CheckPassword(current);

    new PasswordValidator(passwordSettings, "Password").ValidateAndThrow(password);

    ApplyChange(new UserPasswordChangedEvent
    {
      Password = new Pbkdf2(password)
    }, actorId: Id.Value);
  }
  protected virtual void Apply(UserPasswordChangedEvent change) => _password = change.Password;

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

  public void SetPassword(IPasswordSettings passwordSettings, string password)
  {
    new PasswordValidator(passwordSettings, "Password").ValidateAndThrow(password);

    UserUpdatedEvent updated = GetLatestUpdatedEvent();
    updated.Password = new Pbkdf2(password);
    Apply(updated);
  }

  public void SetUniqueName(IUniqueNameSettings uniqueNameSettings, string uniqueName)
  {
    new UniqueNameValidator(uniqueNameSettings, nameof(UniqueName)).ValidateAndThrow(uniqueName);

    UserUpdatedEvent updated = GetLatestUpdatedEvent();
    updated.UniqueName = uniqueName.Trim();
    Apply(updated);
  }

  public SessionAggregate SignIn(IUserSettings userSettings, bool isPersistent = false)
    => SignIn(userSettings, password: null, isPersistent);
  public SessionAggregate SignIn(IUserSettings userSettings, string? password, bool isPersistent = false)
  {
    CheckStatus(userSettings);

    if (password != null)
    {
      CheckPassword(password);
    }

    DateTime now = DateTime.UtcNow;
    ApplyChange(new UserSignedInEvent(), actorId: Id.Value, occurredOn: now);

    return new SessionAggregate(this, isPersistent, now);
  }
  protected virtual void Apply(UserSignedInEvent signedIn) => AuthenticatedOn = signedIn.OccurredOn;

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
  }
  protected UserUpdatedEvent GetLatestUpdatedEvent()
  {
    UserUpdatedEvent? updated = Changes.LastOrDefault(e => e is UserUpdatedEvent) as UserUpdatedEvent;
    if (updated == null)
    {
      updated = new();
      ApplyChange(updated);
    }

    return updated;
  }

  protected virtual void CheckPassword(string password)
  {
    if (_password?.IsMatch(password) != true)
    {
      StringBuilder message = new();
      message.AppendLine("The specified password does not match the user.");
      message.Append("User: ").AppendLine(ToString());
      message.Append("Password: ").AppendLine(password);
      throw new InvalidCredentialsException(message.ToString());
    }
  }
  protected virtual void CheckStatus(IUserSettings userSettings)
  {
    if (IsDisabled)
    {
      throw new UserIsDisabledException(this);
    }
    else if (userSettings.RequireConfirmedAccount && !IsConfirmed)
    {
      throw new UserIsNotConfirmedException(this);
    }
  }

  public override string ToString() => $"{FullName ?? UniqueName} | {base.ToString()}";
}
