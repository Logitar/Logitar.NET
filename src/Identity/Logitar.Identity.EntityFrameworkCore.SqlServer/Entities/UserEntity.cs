using Logitar.Identity.Domain.Users.Events;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Actors;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Constants;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;

public record UserEntity : AggregateEntity
{
  public UserEntity(UserCreatedEvent created, ActorEntity actor) : base(created, actor)
  {
    TenantId = created.TenantId;

    UniqueName = created.UniqueName;
  }

  private UserEntity() : base()
  {
  }

  public int UserId { get; private set; }

  public string? TenantId { get; private set; }

  public string UniqueName { get; private set; } = string.Empty;
  public string UniqueNameNormalized
  {
    get => UniqueName.ToUpper();
    private set { }
  }

  public string? Password { get; private set; }
  public bool HasPassword
  {
    get => Password != null;
    private set { }
  }
  public string? PasswordChangedById { get; private set; }
  public string? PasswordChangedBy { get; private set; }
  public DateTime? PasswordChangedOn { get; private set; }

  public string? DisabledById { get; private set; }
  public string? DisabledBy { get; private set; }
  public DateTime? DisabledOn { get; private set; }
  public bool IsDisabled { get; private set; }

  public string? AddressStreet { get; private set; }
  public string? AddressLocality { get; private set; }
  public string? AddressCountry { get; private set; }
  public string? AddressRegion { get; private set; }
  public string? AddressPostalCode { get; private set; }
  public string? AddressFormatted { get; private set; }
  public string? AddressVerifiedById { get; private set; }
  public string? AddressVerifiedBy { get; private set; }
  public DateTime? AddressVerifiedOn { get; private set; }
  public bool IsAddressVerified { get; private set; }

  public string? EmailAddress { get; private set; }
  public string? EmailAddressNormalized
  {
    get => EmailAddress?.ToUpper();
    private set { }
  }
  public string? EmailVerifiedById { get; private set; }
  public string? EmailVerifiedBy { get; private set; }
  public DateTime? EmailVerifiedOn { get; private set; }
  public bool IsEmailVerified { get; private set; }

  public string? PhoneCountryCode { get; private set; }
  public string? PhoneNumber { get; private set; }
  public string? PhoneExtension { get; private set; }
  public string? PhoneE164Formatted { get; private set; }
  public string? PhoneVerifiedById { get; private set; }
  public string? PhoneVerifiedBy { get; private set; }
  public DateTime? PhoneVerifiedOn { get; private set; }
  public bool IsPhoneVerified { get; private set; }

  public bool IsConfirmed
  {
    get => IsAddressVerified || IsEmailVerified || IsPhoneVerified;
    private set { }
  }

  public DateTime? AuthenticatedOn { get; private set; }

  public string? FirstName { get; private set; }
  public string? MiddleName { get; private set; }
  public string? LastName { get; private set; }
  public string? FullName { get; private set; }
  public string? Nickname { get; private set; }

  public DateTime? Birthdate { get; private set; }
  public string? Gender { get; private set; }
  public string? Locale { get; private set; }
  public string? TimeZone { get; private set; }

  public string? Profile { get; private set; }
  public string? Picture { get; private set; }
  public string? Website { get; private set; }

  public List<SessionEntity> Sessions { get; private set; } = new();

  public void Authenticate(UserAuthenticatedEvent authenticated)
  {
    SetVersion(authenticated);

    AuthenticatedOn = authenticated.OccurredOn;
  }

  public void ChangePassword(UserPasswordChangedEvent change, ActorEntity actor)
  {
    SetVersion(change);

    Password = change.Password.ToString();
    PasswordChangedById = change.ActorId ?? Actor.DefaultId;
    PasswordChangedBy = actor.Serialize();
    PasswordChangedOn = change.OccurredOn;
  }

  public void Disable(UserDisabledEvent disabled, ActorEntity actor)
  {
    SetVersion(disabled);

    DisabledById = disabled.ActorId;
    DisabledBy = actor.Serialize();
    DisabledOn = disabled.OccurredOn;
    IsDisabled = true;
  }

  public void Enable(UserEnabledEvent enabled, ActorEntity actor)
  {
    Update(enabled, actor);

    DisabledById = null;
    DisabledBy = null;
    DisabledOn = null;
    IsDisabled = false;
  }

  public override void SetActor(string id, string json)
  {
    base.SetActor(id, json);

    if (PasswordChangedById == id)
    {
      PasswordChangedBy = json;
    }

    if (DisabledById == id)
    {
      DisabledBy = json;
    }

    if (AddressVerifiedById == id)
    {
      AddressVerifiedBy = json;
    }

    if (EmailVerifiedById == id)
    {
      EmailVerifiedBy = json;
    }

    if (PhoneVerifiedById == id)
    {
      PhoneVerifiedBy = json;
    }
  }

  public void SignIn(UserSignedInEvent signedIn)
  {
    SetVersion(signedIn);

    AuthenticatedOn = signedIn.OccurredOn;
  }

  public void Update(UserUpdatedEvent updated, ActorEntity actor)
  {
    base.Update(updated, actor);

    if (updated.UniqueName != null)
    {
      UniqueName = updated.UniqueName;
    }
    if (updated.Password != null)
    {
      Password = updated.Password.ToString();
      PasswordChangedById = updated.ActorId ?? Actor.DefaultId;
      PasswordChangedBy = actor.Serialize();
      PasswordChangedOn = updated.OccurredOn;
    }

    if (updated.Address != null)
    {
      AddressStreet = updated.Address.Value?.Street;
      AddressLocality = updated.Address.Value?.Locality;
      AddressCountry = updated.Address.Value?.Country;
      AddressRegion = updated.Address.Value?.Region;
      AddressPostalCode = updated.Address.Value?.PostalCode;
      AddressFormatted = updated.Address.Value?.Format();

      if (updated.Address.Value == null || !updated.Address.Value.IsVerified)
      {
        AddressVerifiedById = null;
        AddressVerifiedBy = null;
        AddressVerifiedOn = null;
        IsAddressVerified = false;
      }
      else if (!IsAddressVerified && updated.Address.Value.IsVerified)
      {
        AddressVerifiedById = updated.ActorId ?? Actor.DefaultId;
        AddressVerifiedBy = actor.Serialize();
        AddressVerifiedOn = updated.OccurredOn;
        IsAddressVerified = true;
      }
    }
    if (updated.Email != null)
    {
      EmailAddress = updated.Email.Value?.Address;

      if (updated.Email.Value == null || !updated.Email.Value.IsVerified)
      {
        EmailVerifiedById = null;
        EmailVerifiedBy = null;
        EmailVerifiedOn = null;
        IsEmailVerified = false;
      }
      else if (!IsEmailVerified && updated.Email.Value.IsVerified)
      {
        EmailVerifiedById = updated.ActorId ?? Actor.DefaultId;
        EmailVerifiedBy = actor.Serialize();
        EmailVerifiedOn = updated.OccurredOn;
        IsEmailVerified = true;
      }
    }
    if (updated.Phone != null)
    {
      PhoneCountryCode = updated.Phone.Value?.CountryCode;
      PhoneNumber = updated.Phone.Value?.Number;
      PhoneExtension = updated.Phone.Value?.Extension;
      PhoneE164Formatted = updated.Phone.Value?.FormatToE164();

      if (updated.Phone.Value == null || !updated.Phone.Value.IsVerified)
      {
        PhoneVerifiedById = null;
        PhoneVerifiedBy = null;
        PhoneVerifiedOn = null;
        IsPhoneVerified = false;
      }
      else if (!IsPhoneVerified && updated.Phone.Value.IsVerified)
      {
        PhoneVerifiedById = updated.ActorId ?? Actor.DefaultId;
        PhoneVerifiedBy = actor.Serialize();
        PhoneVerifiedOn = updated.OccurredOn;
        IsPhoneVerified = true;
      }
    }

    if (updated.FirstName != null)
    {
      FirstName = updated.FirstName.Value;
    }
    if (updated.MiddleName != null)
    {
      MiddleName = updated.MiddleName.Value;
    }
    if (updated.LastName != null)
    {
      LastName = updated.LastName.Value;
    }
    if (updated.FullName != null)
    {
      FullName = updated.FullName.Value;
    }
    if (updated.Nickname != null)
    {
      Nickname = updated.Nickname.Value;
    }

    if (updated.Birthdate != null)
    {
      Birthdate = updated.Birthdate.Value?.ToUniversalTime();
    }
    if (updated.Gender != null)
    {
      Gender = updated.Gender.Value?.Value;
    }
    if (updated.Locale != null)
    {
      Locale = updated.Locale.Value?.Name;
    }
    if (updated.TimeZone != null)
    {
      TimeZone = updated.TimeZone.Value?.Id;
    }

    if (updated.Picture != null)
    {
      Picture = updated.Picture.Value?.ToString();
    }
    if (updated.Profile != null)
    {
      Profile = updated.Profile.Value?.ToString();
    }
    if (updated.Website != null)
    {
      Website = updated.Website.Value?.ToString();
    }
  }
}
