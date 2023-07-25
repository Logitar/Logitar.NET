using Logitar.Identity.EntityFrameworkCore.SqlServer.Constants;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Configurations;

public class UserConfiguration : AggregateConfiguration<UserEntity>, IEntityTypeConfiguration<UserEntity>
{
  public override void Configure(EntityTypeBuilder<UserEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(nameof(IdentityContext.Users));
    builder.HasKey(x => x.UserId);
    builder.HasIndex(x => x.PasswordChangedById);
    builder.HasIndex(x => x.DisabledById);
    builder.HasIndex(x => x.EmailVerifiedById);
    builder.HasIndex(x => x.PhoneVerifiedById);
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
  }
}
