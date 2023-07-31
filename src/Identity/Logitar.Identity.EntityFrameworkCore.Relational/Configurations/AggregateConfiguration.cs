using Logitar.Identity.EntityFrameworkCore.Relational.Constants;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Configurations;

public abstract class AggregateConfiguration<T> where T : AggregateEntity
{
  public virtual void Configure(EntityTypeBuilder<T> builder)
  {
    builder.HasIndex(x => x.AggregateId).IsUnique();
    builder.HasIndex(x => x.CreatedById);
    builder.HasIndex(x => x.CreatedOn);
    builder.HasIndex(x => x.UpdatedById);
    builder.HasIndex(x => x.UpdatedOn);
    builder.HasIndex(x => x.Version);

    builder.Property(x => x.AggregateId).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.CreatedById).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.CreatedBy).HasMaxLength(Actor.SerializedLength);
    builder.Property(x => x.UpdatedById).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.UpdatedBy).HasMaxLength(Actor.SerializedLength);
  }
}
