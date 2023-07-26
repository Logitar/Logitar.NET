using Logitar.Data;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Constants;

public static class Db
{
  public static class Events
  {
    public static readonly TableId Table = new(nameof(EventContext.Events));

    public static readonly ColumnId AggregateId = new(nameof(EventEntity.AggregateId), Table);
    public static readonly ColumnId AggregateType = new(nameof(EventEntity.AggregateType), Table);
  }

  public static class Sessions
  {
    public static readonly TableId Table = new(nameof(IdentityContext.Sessions));

    public static readonly ColumnId AggregateId = new(nameof(SessionEntity.AggregateId), Table);
    public static readonly ColumnId IsActive = new(nameof(SessionEntity.IsActive), Table);
    public static readonly ColumnId IsPersistent = new(nameof(SessionEntity.IsPersistent), Table);
    public static readonly ColumnId UserId = new(nameof(SessionEntity.UserId), Table);
  }

  public static class Users
  {
    public static readonly TableId Table = new(nameof(IdentityContext.Users));

    public static readonly ColumnId AddressFormatted = new(nameof(UserEntity.AggregateId), Table);
    public static readonly ColumnId AggregateId = new(nameof(UserEntity.AggregateId), Table);
    public static readonly ColumnId EmailAddress = new(nameof(UserEntity.EmailAddress), Table);
    public static readonly ColumnId EmailAddressNormalized = new(nameof(UserEntity.EmailAddressNormalized), Table);
    public static readonly ColumnId FullName = new(nameof(UserEntity.EmailAddressNormalized), Table);
    public static readonly ColumnId HasPassword = new(nameof(UserEntity.HasPassword), Table);
    public static readonly ColumnId IsConfirmed = new(nameof(UserEntity.IsConfirmed), Table);
    public static readonly ColumnId IsDisabled = new(nameof(UserEntity.IsDisabled), Table);
    public static readonly ColumnId PhoneE164Formatted = new(nameof(UserEntity.EmailAddressNormalized), Table);
    public static readonly ColumnId TenantId = new(nameof(UserEntity.TenantId), Table);
    public static readonly ColumnId UniqueName = new(nameof(UserEntity.UniqueName), Table);
    public static readonly ColumnId UniqueNameNormalized = new(nameof(UserEntity.UniqueNameNormalized), Table);
    public static readonly ColumnId UserId = new(nameof(UserEntity.UserId), Table);
  }
}
