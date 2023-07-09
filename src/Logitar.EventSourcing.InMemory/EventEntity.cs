using Logitar.EventSourcing.Infrastructure;

namespace Logitar.EventSourcing.InMemory;

public class EventEntity : IEventEntity
{
  private EventEntity()
  {
  }

  public long EventId { get; private set; }
  public Guid Id { get; private set; }

  public string? ActorId { get; private set; }
  public DateTime OccurredOn { get; private set; }
  public long Version { get; private set; }
  public DeleteAction DeleteAction { get; private set; }

  public string AggregateType { get; private set; } = string.Empty;
  public string AggregateId { get; private set; } = string.Empty;

  public string EventType { get; private set; } = string.Empty;
  public string EventData { get; private set; } = string.Empty;

  public static IEnumerable<EventEntity> FromChanges(AggregateRoot aggregate)
  {
    string aggregateId = aggregate.Id.Value;
    string aggregateType = aggregate.GetType().GetName();

    return aggregate.Changes.Select(change => new EventEntity
    {
      Id = change.Id,
      ActorId = change.ActorId,
      OccurredOn = change.OccurredOn.ToUniversalTime(),
      Version = change.Version,
      DeleteAction = change.DeleteAction,
      AggregateType = aggregateType,
      AggregateId = aggregateId,
      EventType = change.GetType().GetName(),
      EventData = EventSerializer.Instance.Serialize(change)
    });
  }
}
