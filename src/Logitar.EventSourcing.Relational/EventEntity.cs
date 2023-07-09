using Logitar.EventSourcing.Infrastructure;

namespace Logitar.EventSourcing.Relational;

public class EventEntity : IEventEntity
{
  public long EventId { get; set; }
  public Guid Id { get; set; }

  public string? ActorId { get; set; }
  public DateTime OccurredOn { get; set; }
  public long Version { get; set; }
  public DeleteAction DeleteAction { get; set; }

  public string AggregateType { get; set; } = string.Empty;
  public string AggregateId { get; set; } = string.Empty;

  public string EventType { get; set; } = string.Empty;
  public string EventData { get; set; } = string.Empty;
}
