using Logitar.Identity.EntityFrameworkCore.Relational.Constants;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Configurations;

public class ExternalIdentifierConfiguration : IEntityTypeConfiguration<ExternalIdentifierEntity>
{
  public void Configure(EntityTypeBuilder<ExternalIdentifierEntity> builder)
  {
    builder.ToTable(nameof(IdentityContext.ExternalIdentifiers));
    builder.HasKey(x => x.ExternalIdentifierId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.CreatedById);
    builder.HasIndex(x => x.UpdatedById);
    builder.HasIndex(x => new { x.TenantId, x.Key, x.ValueNormalized }).IsUnique();

    builder.Property(x => x.TenantId).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.Key).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.Value).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.ValueNormalized).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.CreatedById).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.CreatedBy).HasMaxLength(Actor.SerializedLength);
    builder.Property(x => x.UpdatedById).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.UpdatedBy).HasMaxLength(Actor.SerializedLength);
  }
}
