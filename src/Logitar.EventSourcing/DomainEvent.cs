namespace Logitar.EventSourcing;

/// <summary>
/// Represents a domain event that has been raised by an <see cref="AggregateRoot"/> and can be applied to it.
/// </summary>
public record DomainEvent
{
  /// <summary>
  /// Gets or sets the identifier of the event.
  /// </summary>
  public Guid? Id { get; set; }

  /// <summary>
  /// Gets or sets the identifier of the aggregate to which the event belongs to.
  /// </summary>
  public AggregateId? AggregateId { get; set; }
  /// <summary>
  /// Gets or sets the version of the event.
  /// </summary>
  public long Version { get; set; }

  /// <summary>
  /// Gets or sets the identifier of the actor who triggered the event.
  /// </summary>
  public string? ActorId { get; set; }
  /// <summary>
  /// Gets or sets the date and time when the event occurred.
  /// </summary>
  public DateTime OccurredOn { get; set; }

  /// <summary>
  /// Gets or sets the delete action performed by the event.
  /// </summary>
  public DeleteAction DeleteAction { get; set; }
}
