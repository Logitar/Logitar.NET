using Logitar.Identity.Domain.Users;
using Logitar.Identity.Domain.Users.Events;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Entities;

public record UserEntity : AggregateEntity
{
  public UserEntity(UserCreatedEvent created) : base(created)
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
  public string? PasswordChangedBy { get; private set; }
  public DateTime? PasswordChangedOn { get; private set; }

  public string? DisabledBy { get; private set; }
  public DateTime? DisabledOn { get; private set; }
  public bool IsDisabled { get; private set; }

  public DateTime? AuthenticatedOn { get; private set; }

  public string? AddressStreet { get; private set; }
  public string? AddressLocality { get; private set; }
  public string? AddressRegion { get; private set; }
  public string? AddressPostalCode { get; private set; }
  public string? AddressCountry { get; private set; }
  public string? AddressFormatted { get; private set; }
  public string? AddressVerifiedBy { get; private set; }
  public DateTime? AddressVerifiedOn { get; private set; }
  public bool IsAddressVerified { get; private set; }

  public string? EmailAddress { get; private set; }
  public string? EmailAddressNormalized
  {
    get => EmailAddress?.ToUpper();
    private set { }
  }
  public string? EmailVerifiedBy { get; private set; }
  public DateTime? EmailVerifiedOn { get; private set; }
  public bool IsEmailVerified { get; private set; }

  public string? PhoneCountryCode { get; private set; }
  public string? PhoneNumber { get; private set; }
  public string? PhoneExtension { get; private set; }
  public string? PhoneE164Formatted { get; private set; }
  public string? PhoneVerifiedBy { get; private set; }
  public DateTime? PhoneVerifiedOn { get; private set; }
  public bool IsPhoneVerified { get; private set; }

  public bool IsConfirmed
  {
    get => IsAddressVerified || IsEmailVerified || IsPhoneVerified;
    private set { }
  }

  public string? FirstName { get; private set; }
  public string? MiddleName { get; private set; }
  public string? LastName { get; private set; }
  public string? FullName { get; private set; }
  public string? Nickname { get; private set; }

  public DateTime? Birthdate { get; private set; }
  public string? Gender { get; private set; }
  public string? Locale { get; private set; }
  public string? TimeZone { get; private set; }

  public string? Picture { get; private set; }
  public string? Profile { get; private set; }
  public string? Website { get; private set; }

  public Dictionary<string, string> CustomAttributes { get; private set; } = new();
  public string? CustomAttributesSerialized
  {
    get => CustomAttributes.Any() ? JsonSerializer.Serialize(CustomAttributes) : null;
    private set
    {
      if (value == null)
      {
        CustomAttributes.Clear();
      }
      else
      {
        CustomAttributes = JsonSerializer.Deserialize<Dictionary<string, string>>(value) ?? new();
      }
    }
  }

  public List<RoleEntity> Roles { get; private set; } = new();
  public List<SessionEntity> Sessions { get; private set; } = new();

  public void ChangePassword(UserPasswordChangedEvent changed)
  {
    Update(changed);

    Password = changed.Password.Encode();
    PasswordChangedBy = changed.ActorId.Value;
    PasswordChangedOn = changed.OccurredOn.ToUniversalTime();
  }

  public void Disable(UserDisabledEvent disabled)
  {
    Update(disabled);

    DisabledBy = disabled.ActorId.Value;
    DisabledOn = disabled.OccurredOn.ToUniversalTime();
    IsDisabled = true;
  }

  public void Enable(UserEnabledEvent enabled)
  {
    Update(enabled);

    DisabledBy = null;
    DisabledOn = null;
    IsDisabled = false;
  }

  public void SignIn(UserSignedInEvent signedIn)
  {
    Update(signedIn);

    AuthenticatedOn = signedIn.OccurredOn.ToUniversalTime();
  }

  public void Update(UserUpdatedEvent updated, IEnumerable<RoleEntity> roles)
  {
    base.Update(updated);

    if (updated.UniqueName != null)
    {
      UniqueName = updated.UniqueName;
    }
    if (updated.Password != null)
    {
      Password = updated.Password.Encode();
      PasswordChangedBy = updated.ActorId.Value;
      PasswordChangedOn = updated.OccurredOn.ToUniversalTime();
    }

    if (updated.Address != null)
    {
      AddressStreet = updated.Address.Value?.Street;
      AddressLocality = updated.Address.Value?.Locality;
      AddressRegion = updated.Address.Value?.Region;
      AddressPostalCode = updated.Address.Value?.PostalCode;
      AddressCountry = updated.Address.Value?.Country;
      AddressFormatted = updated.Address.Value == null ? null : PostalAddressHelper.Format(updated.Address.Value);

      if (!IsAddressVerified && updated.Address.Value?.IsVerified == true)
      {
        AddressVerifiedBy = updated.ActorId.Value;
        AddressVerifiedOn = updated.OccurredOn.ToUniversalTime();
        IsAddressVerified = true;
      }
      else if (IsAddressVerified && updated.Address.Value?.IsVerified != true)
      {
        AddressVerifiedBy = null;
        AddressVerifiedOn = null;
        IsAddressVerified = false;
      }
    }
    if (updated.Email != null)
    {
      EmailAddress = updated.Email.Value?.Address;

      if (!IsEmailVerified && updated.Email.Value?.IsVerified == true)
      {
        EmailVerifiedBy = updated.ActorId.Value;
        EmailVerifiedOn = updated.OccurredOn.ToUniversalTime();
        IsEmailVerified = true;
      }
      else if (IsEmailVerified && updated.Email.Value?.IsVerified != true)
      {
        EmailVerifiedBy = null;
        EmailVerifiedOn = null;
        IsEmailVerified = false;
      }
    }
    if (updated.Phone != null)
    {
      PhoneCountryCode = updated.Phone.Value?.CountryCode;
      PhoneNumber = updated.Phone.Value?.Number;
      PhoneExtension = updated.Phone.Value?.Extension;
      PhoneE164Formatted = updated.Phone.Value?.FormatToE164();

      if (!IsPhoneVerified && updated.Phone.Value?.IsVerified == true)
      {
        PhoneVerifiedBy = updated.ActorId.Value;
        PhoneVerifiedOn = updated.OccurredOn.ToUniversalTime();
        IsPhoneVerified = true;
      }
      else if (IsPhoneVerified && updated.Phone.Value?.IsVerified != true)
      {
        PhoneVerifiedBy = null;
        PhoneVerifiedOn = null;
        IsPhoneVerified = false;
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
      Locale = updated.Locale.Value?.Code;
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

    foreach (KeyValuePair<string, string?> customAttribute in updated.CustomAttributes)
    {
      if (customAttribute.Value == null)
      {
        CustomAttributes.Remove(customAttribute.Key);
      }
      else
      {
        CustomAttributes[customAttribute.Key] = customAttribute.Value;
      }
    }

    Dictionary<string, RoleEntity> rolesById = roles.GroupBy(x => x.AggregateId)
      .ToDictionary(x => x.Key, x => x.Last());
    foreach (KeyValuePair<string, Domain.Action> roleAction in updated.Roles)
    {
      if (rolesById.TryGetValue(roleAction.Key, out RoleEntity? role))
      {
        switch (roleAction.Value)
        {
          case Domain.Action.Add:
            Roles.Add(role);
            break;
          case Domain.Action.Remove:
            Roles.Remove(role);
            break;
        }
      }
    }
  }
}
