using System.Text;

namespace Logitar.EventSourcing.Infrastructure;

/// <summary>
/// The exception thrown when the type of an event could not be found.
/// </summary>
public class EventTypeNotFoundException : Exception
{
  /// <summary>
  /// Initializes a new instance of the <see cref="EventTypeNotFoundException"/> class.
  /// </summary>
  /// <param name="entity">The invalid event.</param>
  internal EventTypeNotFoundException(IEventEntity entity) : base(BuildMessage(entity))
  {
    Data[nameof(EventId)] = entity.Id;
    Data[nameof(TypeName)] = entity.EventType;
  }

  /// <summary>
  /// Gets the identifier of the invalid event.
  /// </summary>
  public Guid EventId => (Guid)Data[nameof(EventId)]!;
  /// <summary>
  /// Gets the name of the type that could not be found.
  /// </summary>
  public string TypeName => (string)Data[nameof(TypeName)]!;

  /// <summary>
  /// Builds the exception message.
  /// </summary>
  /// <param name="entity">The invalid event.</param>
  /// <returns>The exception message.</returns>
  private static string BuildMessage(IEventEntity entity)
  {
    StringBuilder message = new();

    message.AppendLine("The specified event type could not be found.");
    message.Append("EventId: ").Append(entity.Id).AppendLine();
    message.Append("TypeName: ").AppendLine(entity.EventType);

    return message.ToString();
  }
}
