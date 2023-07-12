namespace Logitar.EventSourcing.Infrastructure;

/// <summary>
/// The exception thrown when the deserialization of an event's data failed or returned null.
/// </summary>
public class EventDataDeserializationFailedException : Exception
{
  /// <summary>
  /// Initializes a new instance of the <see cref="EventDataDeserializationFailedException"/> class.
  /// </summary>
  /// <param name="entity">The invalid event.</param>
  internal EventDataDeserializationFailedException(IEventEntity entity) : base(BuildMessage(entity))
  {
    Data[nameof(EventId)] = entity.Id;
    Data[nameof(EventType)] = entity.EventType;
    Data[nameof(EventData)] = entity.EventData;
  }

  /// <summary>
  /// Gets the identifier of the invalid event.
  /// </summary>
  public Guid EventId => (Guid)Data[nameof(EventId)]!;
  /// <summary>
  /// Gets the type of the invalid event.
  /// </summary>
  public string EventType => (string)Data[nameof(EventType)]!;
  /// <summary>
  /// Gets the data of the invalid event.
  /// </summary>
  public string EventData => (string)Data[nameof(EventData)]!;

  /// <summary>
  /// Builds the exception message.
  /// </summary>
  /// <param name="entity">The invalid event.</param>
  /// <returns>The exception message</returns>
  private static string BuildMessage(IEventEntity entity)
  {
    StringBuilder message = new();

    message.AppendLine("The specified event data could not be deserialized.");
    message.Append("EventId: ").Append(entity.Id).AppendLine();
    message.Append("EventType: ").AppendLine(entity.EventType);
    message.Append("EventData: ").AppendLine(entity.EventData);

    return message.ToString();
  }
}
