using Logitar.Data;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

public static class Db
{
  public static class Events
  {
    public static readonly TableId Table = new(nameof(EventContext.Events));

    public static readonly ColumnId AggregateId = new(nameof(EventEntity.AggregateId), Table);
    public static readonly ColumnId AggregateType = new(nameof(EventEntity.AggregateType), Table);
  }

  public static class Roles
  {
    public static readonly TableId Table = new(nameof(IdentityContext.Roles));

    public static readonly ColumnId AggregateId = new(nameof(RoleEntity.AggregateId), Table);
    public static readonly ColumnId DisplayName = new(nameof(RoleEntity.DisplayName), Table);
    public static readonly ColumnId RoleId = new(nameof(RoleEntity.RoleId), Table);
    public static readonly ColumnId TenantId = new(nameof(RoleEntity.TenantId), Table);
    public static readonly ColumnId UniqueName = new(nameof(RoleEntity.UniqueName), Table);
    public static readonly ColumnId UniqueNameNormalized = new(nameof(RoleEntity.UniqueNameNormalized), Table);
  }

  public static class Sessions
  {
    public static readonly TableId Table = new(nameof(IdentityContext.Sessions));

    public static readonly ColumnId AggregateId = new(nameof(SessionEntity.AggregateId), Table);
    public static readonly ColumnId IsActive = new(nameof(SessionEntity.IsActive), Table);
    public static readonly ColumnId IsPersistent = new(nameof(SessionEntity.IsPersistent), Table);
    public static readonly ColumnId SessionId = new(nameof(SessionEntity.SessionId), Table);
    public static readonly ColumnId SignedOutBy = new(nameof(SessionEntity.SignedOutBy), Table);
    public static readonly ColumnId SignedOutOn = new(nameof(SessionEntity.SignedOutOn), Table);
    public static readonly ColumnId UserId = new(nameof(SessionEntity.UserId), Table);
  }

  public static class UserRoles
  {
    public static readonly TableId Table = new(nameof(IdentityContext.UserRoles));

    public static readonly ColumnId RoleId = new(nameof(UserRoleEntity.RoleId), Table);
    public static readonly ColumnId UserId = new(nameof(UserRoleEntity.UserId), Table);
  }

  public static class Users
  {
    public static readonly TableId Table = new(nameof(IdentityContext.Users));

    public static readonly ColumnId AddressFormatted = new(nameof(UserEntity.AddressFormatted), Table);
    public static readonly ColumnId AddressVerifiedBy = new(nameof(UserEntity.AddressVerifiedBy), Table);
    public static readonly ColumnId AggregateId = new(nameof(UserEntity.AggregateId), Table);
    public static readonly ColumnId AuthenticatedOn = new(nameof(UserEntity.AuthenticatedOn), Table);
    public static readonly ColumnId Birthdate = new(nameof(UserEntity.Birthdate), Table);
    public static readonly ColumnId DisabledBy = new(nameof(UserEntity.DisabledBy), Table);
    public static readonly ColumnId DisabledOn = new(nameof(UserEntity.DisabledOn), Table);
    public static readonly ColumnId EmailAddress = new(nameof(UserEntity.EmailAddress), Table);
    public static readonly ColumnId EmailAddressNormalized = new(nameof(UserEntity.EmailAddressNormalized), Table);
    public static readonly ColumnId EmailVerifiedBy = new(nameof(UserEntity.EmailVerifiedBy), Table);
    public static readonly ColumnId FirstName = new(nameof(UserEntity.FirstName), Table);
    public static readonly ColumnId FullName = new(nameof(UserEntity.FullName), Table);
    public static readonly ColumnId Gender = new(nameof(UserEntity.Gender), Table);
    public static readonly ColumnId HasPassword = new(nameof(UserEntity.HasPassword), Table);
    public static readonly ColumnId IsConfirmed = new(nameof(UserEntity.IsConfirmed), Table);
    public static readonly ColumnId IsDisabled = new(nameof(UserEntity.IsDisabled), Table);
    public static readonly ColumnId LastName = new(nameof(UserEntity.LastName), Table);
    public static readonly ColumnId Locale = new(nameof(UserEntity.Locale), Table);
    public static readonly ColumnId MiddleName = new(nameof(UserEntity.MiddleName), Table);
    public static readonly ColumnId Nickname = new(nameof(UserEntity.Nickname), Table);
    public static readonly ColumnId PasswordChangedBy = new(nameof(UserEntity.PasswordChangedBy), Table);
    public static readonly ColumnId PasswordChangedOn = new(nameof(UserEntity.PasswordChangedOn), Table);
    public static readonly ColumnId PhoneE164Formatted = new(nameof(UserEntity.PhoneE164Formatted), Table);
    public static readonly ColumnId PhoneVerifiedBy = new(nameof(UserEntity.PhoneVerifiedBy), Table);
    public static readonly ColumnId TenantId = new(nameof(UserEntity.TenantId), Table);
    public static readonly ColumnId TimeZone = new(nameof(UserEntity.TimeZone), Table);
    public static readonly ColumnId UniqueName = new(nameof(UserEntity.UniqueName), Table);
    public static readonly ColumnId UniqueNameNormalized = new(nameof(UserEntity.UniqueNameNormalized), Table);
    public static readonly ColumnId UserId = new(nameof(UserEntity.UserId), Table);
  }
}
