using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Configurations;

public class RoleConfiguration : AggregateConfiguration<RoleEntity>, IEntityTypeConfiguration<RoleEntity>
{
  public override void Configure(EntityTypeBuilder<RoleEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(nameof(IdentityContext.Roles));
    builder.HasKey(x => x.RoleId);

    builder.HasIndex(x => x.TenantId);
    builder.HasIndex(x => x.UniqueName);
    builder.HasIndex(x => x.DisplayName);
    builder.HasIndex(x => new { x.TenantId, x.UniqueNameNormalized }).IsUnique();

    builder.Property(x => x.TenantId).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.UniqueName).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.UniqueNameNormalized).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.DisplayName).HasMaxLength(byte.MaxValue);
  }
}
