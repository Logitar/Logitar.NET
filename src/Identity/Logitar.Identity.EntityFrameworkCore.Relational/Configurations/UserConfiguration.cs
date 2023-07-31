﻿using Logitar.Identity.EntityFrameworkCore.Relational.Constants;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Configurations;

public class UserConfiguration : AggregateConfiguration<UserEntity>, IEntityTypeConfiguration<UserEntity>
{
  public override void Configure(EntityTypeBuilder<UserEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(nameof(IdentityContext.Users));
    builder.HasKey(x => x.UserId);

    builder.HasIndex(x => x.TenantId);
    builder.HasIndex(x => x.UniqueName);
    builder.HasIndex(x => x.HasPassword);
    builder.HasIndex(x => x.PasswordChangedById);
    builder.HasIndex(x => x.PasswordChangedOn);
    builder.HasIndex(x => x.DisabledById);
    builder.HasIndex(x => x.DisabledOn);
    builder.HasIndex(x => x.IsDisabled);
    builder.HasIndex(x => x.AddressFormatted);
    builder.HasIndex(x => x.AddressVerifiedById);
    builder.HasIndex(x => x.EmailAddress);
    builder.HasIndex(x => x.EmailVerifiedById);
    builder.HasIndex(x => x.PhoneE164Formatted);
    builder.HasIndex(x => x.PhoneVerifiedById);
    builder.HasIndex(x => x.IsConfirmed);
    builder.HasIndex(x => x.AuthenticatedOn);
    builder.HasIndex(x => x.FirstName);
    builder.HasIndex(x => x.MiddleName);
    builder.HasIndex(x => x.LastName);
    builder.HasIndex(x => x.FullName);
    builder.HasIndex(x => x.Birthdate);
    builder.HasIndex(x => new { x.TenantId, x.UniqueNameNormalized }).IsUnique();
    builder.HasIndex(x => new { x.TenantId, x.EmailAddressNormalized });

    builder.Property(x => x.TenantId).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.UniqueName).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.UniqueNameNormalized).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.Password).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.PasswordChangedById).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.PasswordChangedBy).HasMaxLength(Actor.SerializedLength);
    builder.Property(x => x.DisabledById).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.DisabledBy).HasMaxLength(Actor.SerializedLength);
    builder.Property(x => x.AddressStreet).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.AddressLocality).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.AddressCountry).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.AddressRegion).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.AddressPostalCode).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.AddressFormatted).HasMaxLength(2048);
    builder.Property(x => x.AddressVerifiedById).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.AddressVerifiedBy).HasMaxLength(Actor.SerializedLength);
    builder.Property(x => x.EmailAddress).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.EmailAddressNormalized).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.EmailVerifiedById).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.EmailVerifiedBy).HasMaxLength(Actor.SerializedLength);
    builder.Property(x => x.PhoneCountryCode).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.PhoneNumber).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.PhoneExtension).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.PhoneE164Formatted).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.PhoneVerifiedById).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.PhoneVerifiedBy).HasMaxLength(Actor.SerializedLength);
    builder.Property(x => x.FirstName).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.MiddleName).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.LastName).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.FullName).HasMaxLength(1000);
    builder.Property(x => x.Nickname).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.Gender).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.Locale).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.TimeZone).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.Picture).HasMaxLength(2048);
    builder.Property(x => x.Profile).HasMaxLength(2048);
    builder.Property(x => x.Website).HasMaxLength(2048);

    builder.HasMany(x => x.Roles).WithMany(x => x.Users).UsingEntity<UserRoleEntity>(join =>
    {
      join.ToTable(nameof(IdentityContext.UserRoles));
      join.HasKey(x => new { x.UserId, x.RoleId });
    });
  }
}
