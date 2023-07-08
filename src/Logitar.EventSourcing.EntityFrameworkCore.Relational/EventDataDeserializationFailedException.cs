using System.Text;

namespace Logitar.EventSourcing.EntityFrameworkCore.Relational;

/// <summary>
/// TODO(fpion): document
/// </summary>
public class EventDataDeserializationFailedException : Exception
{
  internal EventDataDeserializationFailedException(EventEntity e) : base(BuildMessage(e))
  {
    Data[nameof(EventId)] = e.Id;
    Data[nameof(EventType)] = e.EventType;
    Data[nameof(EventData)] = e.EventData;
  }

  public Guid EventId => (Guid)Data[nameof(EventId)]!;
  public string EventType => (string)Data[nameof(EventType)]!;
  public string EventData => (string)Data[nameof(EventData)]!;

  private static string BuildMessage(EventEntity e)
  {
    StringBuilder message = new();

    message.AppendLine("The specified event data could not be deserialized.");
    message.Append("EventId: ").Append(e.Id).AppendLine();
    message.Append("EventType: ").AppendLine(e.EventType);
    message.Append("EventData: ").AppendLine(e.EventData);

    return message.ToString();
  }
}
