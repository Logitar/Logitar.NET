namespace Logitar.EventSourcing.Infrastructure;

public class EventEntity
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

    return aggregate.Changes.Select(e => new EventEntity
    {
      Id = e.Id,
      ActorId = e.ActorId,
      OccurredOn = e.OccurredOn.ToUniversalTime(),
      Version = e.Version,
      DeleteAction = e.DeleteAction,
      AggregateType = aggregateType,
      AggregateId = aggregateId,
      EventType = e.GetType().GetName(),
      EventData = EventSerializer.Instance.Serialize(e)
    });
  }
}
