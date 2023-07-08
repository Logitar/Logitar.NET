namespace Logitar.EventSourcing;

/// <summary>
/// The exception thrown when an event is applied to the wrong aggregate.
/// </summary>
public class EventAggregateMismatchException : Exception
{
  /// <summary>
  /// Initializes a new instance of the <see cref="EventAggregateMismatchException"/> class.
  /// </summary>
  /// <param name="aggregate">The aggregate unto which the event was applied.</param>
  /// <param name="change">The event belonging to another aggregate.</param>
  public EventAggregateMismatchException(AggregateRoot aggregate, DomainEvent change) : base(BuildMessage(aggregate, change))
  {
    Data[nameof(Aggregate)] = aggregate.ToString();
    Data[nameof(AggregateId)] = aggregate.Id.ToString();
    Data[nameof(Event)] = change.ToString();
    Data[nameof(EventId)] = change.Id;
    Data[nameof(EventAggregateId)] = change.AggregateId?.ToString();
  }

  /// <summary>
  /// Gets the string representation of the aggregate.
  /// </summary>
  public string Aggregate => (string)Data[nameof(Aggregate)]!;
  /// <summary>
  /// Gets the string representation of the aggregate identifier.
  /// </summary>
  public string AggregateId => (string)Data[nameof(AggregateId)]!;
  /// <summary>
  /// Gets the string representation of the event.
  /// </summary>
  public string Event => (string)Data[nameof(Event)]!;
  /// <summary>
  /// Gets the identifier of the event.
  /// </summary>
  public Guid? EventId => (Guid?)Data[nameof(EventId)];
  /// <summary>
  /// Gets the string representation of the event aggregate identifier.
  /// </summary>
  public string EventAggregateId => (string)Data[nameof(EventAggregateId)]!;

  /// <summary>
  /// Builds the exception message.
  /// </summary>
  /// <param name="aggregate">The aggregate unto which the event was applied.</param>
  /// <param name="change">The event belonging to another aggregate.</param>
  /// <returns>The exception message.</returns>
  private static string BuildMessage(AggregateRoot aggregate, DomainEvent change)
  {
    StringBuilder message = new();

    message.AppendLine("The specified event does not belong to the specified aggregate.");
    message.Append("Aggregate: ").Append(aggregate).AppendLine();
    message.Append("AggregateId: ").Append(aggregate.Id).AppendLine();
    message.Append("Event: ").Append(change).AppendLine();
    message.Append("EventId: ").Append(change.Id).AppendLine();
    message.Append("EventAggregateId: ").Append(change.AggregateId).AppendLine();

    return message.ToString();
  }
}
