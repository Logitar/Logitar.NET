namespace Logitar.EventSourcing;

/// <summary>
/// The exception thrown when a past event is applied to an aggregate of a future state.
/// </summary>
public class CannotApplyPastEventException : Exception
{
  /// <summary>
  /// Initializes a new instance of the <see cref="CannotApplyPastEventException"/> class.
  /// </summary>
  /// <param name="aggregate">The aggregate in a future state.</param>
  /// <param name="change">The event of a past state.</param>
  public CannotApplyPastEventException(AggregateRoot aggregate, DomainEvent change) : base(BuildMessage(aggregate, change))
  {
    Data[nameof(Aggregate)] = aggregate.ToString();
    Data[nameof(AggregateId)] = aggregate.Id.ToString();
    Data[nameof(AggregateVersion)] = aggregate.Version;
    Data[nameof(Event)] = change.ToString();
    Data[nameof(EventId)] = change.Id;
    Data[nameof(EventVersion)] = change.Version;
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
  /// Gets the version of the aggregate.
  /// </summary>
  public long AggregateVersion => (long)Data[nameof(AggregateVersion)]!;
  /// <summary>
  /// Gets the string representation of the event.
  /// </summary>
  public string Event => (string)Data[nameof(Event)]!;
  /// <summary>
  /// Gets the identifier of the event.
  /// </summary>
  public Guid? EventId => (Guid?)Data[nameof(EventId)];
  /// <summary>
  /// Gets the version of the event.
  /// </summary>
  public long? EventVersion => (long?)Data[nameof(EventVersion)];

  /// <summary>
  /// Builds the exception message.
  /// </summary>
  /// <param name="aggregate">The aggregate in a future state.</param>
  /// <param name="change">The event of a past state.</param>
  /// <returns>The exception message.</returns>
  private static string BuildMessage(AggregateRoot aggregate, DomainEvent change)
  {
    StringBuilder message = new();

    message.AppendLine("The specified event is past the current state of the specified aggregate.");
    message.Append("Aggregate: ").Append(aggregate).AppendLine();
    message.Append("AggregateId: ").Append(aggregate.Id).AppendLine();
    message.Append("AggregateVersion: ").Append(aggregate.Version).AppendLine();
    message.Append("Event: ").Append(change).AppendLine();
    message.Append("EventId: ").Append(change.Id).AppendLine();
    message.Append("EventVersion: ").Append(change.Version).AppendLine();

    return message.ToString();
  }
}
