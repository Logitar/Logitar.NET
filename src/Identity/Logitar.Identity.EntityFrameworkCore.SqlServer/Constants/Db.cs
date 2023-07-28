﻿using Logitar.Data;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Constants;

public static class Db
{
  public static class ApiKeyRoles
  {
    public static readonly TableId Table = new(nameof(IdentityContext.ApiKeyRoles));

    public static readonly ColumnId ApiKeyId = new(nameof(ApiKeyRoleEntity.ApiKeyId), Table);
    public static readonly ColumnId RoleId = new(nameof(ApiKeyRoleEntity.RoleId), Table);
  }

  public static class ApiKeys
  {
    public static readonly TableId Table = new(nameof(IdentityContext.ApiKeys));

    public static readonly ColumnId AggregateId = new(nameof(ApiKeyEntity.AggregateId), Table);
    public static readonly ColumnId ApiKeyId = new(nameof(ApiKeyEntity.ApiKeyId), Table);
    public static readonly ColumnId ExpiresOn = new(nameof(ApiKeyEntity.ExpiresOn), Table);
    public static readonly ColumnId TenantId = new(nameof(ApiKeyEntity.TenantId), Table);
    public static readonly ColumnId Title = new(nameof(ApiKeyEntity.Title), Table);
  }

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
