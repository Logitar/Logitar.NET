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

  public bool IsConfirmed
  {
    get => IsEmailVerified;
    private set { }
  }

  public DateTime? AuthenticatedOn { get; private set; }

  public string? FirstName { get; private set; }
  public string? LastName { get; private set; }
  public string? FullName { get; private set; }

  public string? Locale { get; private set; }

  public List<SessionEntity> Sessions { get; private set; } = new();

  public void Authenticate(UserAuthenticatedEvent authenticated) => AuthenticatedOn = authenticated.OccurredOn;

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

  public void SignIn(UserSignedInEvent signedIn) => AuthenticatedOn = signedIn.OccurredOn;

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

    if (updated.FirstName != null)
    {
      FirstName = updated.FirstName.Value;
    }
    if (updated.LastName != null)
    {
      LastName = updated.LastName.Value;
    }
    if (updated.FullName != null)
    {
      FullName = updated.FullName.Value;
    }

    if (updated.Locale != null)
    {
      Locale = updated.Locale.Value?.Name;
    }
  }
}
