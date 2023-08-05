using Logitar.EventSourcing;

namespace Logitar.Demo.Ui.Domain.Events;

public record TodoUpdatedEvent : DomainEvent
{
  public string? Text { get; set; }
  public bool? IsDone { get; set; }
}
