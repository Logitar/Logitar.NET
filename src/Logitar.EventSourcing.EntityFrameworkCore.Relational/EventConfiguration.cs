using Logitar.EventSourcing.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logitar.EventSourcing.EntityFrameworkCore.Relational;

public class EventConfiguration : IEntityTypeConfiguration<EventEntity>
{
  public void Configure(EntityTypeBuilder<EventEntity> builder)
  {
    builder.ToTable(nameof(EventContext.Events));
    builder.HasKey(x => x.EventId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.ActorId);
    builder.HasIndex(x => x.OccurredOn);
    builder.HasIndex(x => x.Version);
    builder.HasIndex(x => x.AggregateId);
    builder.HasIndex(x => x.EventType);
    builder.HasIndex(x => new { x.AggregateType, x.AggregateId });

    builder.Property(x => x.ActorId).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.AggregateType).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.AggregateId).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.EventType).HasMaxLength(byte.MaxValue);
  }
}
