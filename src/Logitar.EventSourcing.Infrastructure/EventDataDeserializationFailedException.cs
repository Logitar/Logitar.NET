using System.Text;

namespace Logitar.EventSourcing.Infrastructure;

public class EventDataDeserializationFailedException : Exception
{
  internal EventDataDeserializationFailedException(IEventEntity entity) : base(BuildMessage(entity))
  {
    Data[nameof(EventId)] = entity.Id;
    Data[nameof(EventType)] = entity.EventType;
    Data[nameof(EventData)] = entity.EventData;
  }

  public Guid EventId => (Guid)Data[nameof(EventId)]!;
  public string EventType => (string)Data[nameof(EventType)]!;
  public string EventData => (string)Data[nameof(EventData)]!;

  private static string BuildMessage(IEventEntity e)
  {
    StringBuilder message = new();

    message.AppendLine("The specified event data could not be deserialized.");
    message.Append("EventId: ").Append(e.Id).AppendLine();
    message.Append("EventType: ").AppendLine(e.EventType);
    message.Append("EventData: ").AppendLine(e.EventData);

    return message.ToString();
  }
}
