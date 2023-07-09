using System.Text;

namespace Logitar.EventSourcing.Infrastructure;

public class EventTypeNotFoundException : Exception
{
  internal EventTypeNotFoundException(IEventEntity entity) : base(BuildMessage(entity))
  {
    Data[nameof(EventId)] = entity.Id;
    Data[nameof(TypeName)] = entity.EventType;
  }

  public Guid EventId => (Guid)Data[nameof(EventId)]!;
  public string TypeName => (string)Data[nameof(TypeName)]!;

  private static string BuildMessage(IEventEntity e)
  {
    StringBuilder message = new();

    message.AppendLine("The specified event type could not be found.");
    message.Append("EventId: ").Append(e.Id).AppendLine();
    message.Append("TypeName: ").AppendLine(e.EventType);

    return message.ToString();
  }
}
