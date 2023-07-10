using Logitar.EventSourcing.Infrastructure;

namespace Logitar.EventSourcing.EntityFrameworkCore.Relational;

/// <summary>
/// Represents the Entity Framework Core relational storage model for events.
/// </summary>
public class EventEntity : IEventEntity
{
  /// <summary>
  /// Initializes a new instance of the <see cref="EventEntity"/> class.
  /// </summary>
  private EventEntity()
  {
  }

  /// <summary>
  /// Gets of sets the primary key of the event.
  /// </summary>
  public long EventId { get; private set; }
  /// <summary>
  /// Gets or sets the identifier of the event.
  /// </summary>
  public Guid Id { get; private set; }

  /// <summary>
  /// Gets or sets the identifier of the actor who triggered the event.
  /// </summary>
  public string? ActorId { get; private set; }
  /// <summary>
  /// Gets or sets the date and time when the event occurred.
  /// </summary>
  public DateTime OccurredOn { get; private set; }
  /// <summary>
  /// Gets or sets the version of the event.
  /// </summary>
  public long Version { get; private set; }
  /// <summary>
  /// Gets or sets the delete action performed by the event.
  /// </summary>
  public DeleteAction DeleteAction { get; private set; }

  /// <summary>
  /// Gets or sets the type of the aggregate to which the event belongs to.
  /// </summary>
  public string AggregateType { get; private set; } = string.Empty;
  /// <summary>
  /// Gets or sets the identifier of the aggregate to which the event belongs to.
  /// </summary>
  public string AggregateId { get; private set; } = string.Empty;

  /// <summary>
  /// Gets or sets the type of the event.
  /// </summary>
  public string EventType { get; private set; } = string.Empty;
  /// <summary>
  /// Gets or sets the data of the event.
  /// </summary>
  public string EventData { get; private set; } = string.Empty;

  /// <summary>
  /// Converts the changes of the specified aggregate to the storage model.
  /// </summary>
  /// <param name="aggregate">The aggregate to convert the changes from.</param>
  /// <returns>The list of storage models.</returns>
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
