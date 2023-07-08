using System.Text;

namespace Logitar.EventSourcing.EntityFrameworkCore.Relational;

/// <summary>
/// TODO(fpion): document
/// </summary>
public class EventTypeNotFoundException : Exception
{
  internal EventTypeNotFoundException(EventEntity e) : base(BuildMessage(e))
  {
    Data[nameof(EventId)] = e.Id;
    Data[nameof(TypeName)] = e.EventType;
  }

  public Guid EventId => (Guid)Data[nameof(EventId)]!;
  public string TypeName => (string)Data[nameof(TypeName)]!;

  private static string BuildMessage(EventEntity e)
  {
    StringBuilder message = new();

    message.AppendLine("The specified event type could not be found.");
    message.Append("EventId: ").Append(e.Id).AppendLine();
    message.Append("TypeName: ").AppendLine(e.EventType);

    return message.ToString();
  }
}
