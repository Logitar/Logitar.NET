using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Configurations;

public class ApiKeyConfiguration : AggregateConfiguration<ApiKeyEntity>, IEntityTypeConfiguration<ApiKeyEntity>
{
  public override void Configure(EntityTypeBuilder<ApiKeyEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(nameof(IdentityContext.ApiKeys));
    builder.HasKey(x => x.ApiKeyId);

    builder.HasIndex(x => x.TenantId);
    builder.HasIndex(x => x.Title);
    builder.HasIndex(x => x.ExpiresOn);

    builder.Property(x => x.TenantId).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.Secret).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.Title).HasMaxLength(byte.MaxValue);

    builder.HasMany(x => x.Roles).WithMany(x => x.ApiKeys).UsingEntity<ApiKeyRoleEntity>(join =>
    {
      join.ToTable(nameof(IdentityContext.ApiKeyRoles));
      join.HasKey(x => new { x.ApiKeyId, x.RoleId });
    });
  }
}
