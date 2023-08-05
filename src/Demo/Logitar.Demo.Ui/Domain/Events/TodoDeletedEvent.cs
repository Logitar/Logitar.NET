using Logitar.EventSourcing;

namespace Logitar.Demo.Ui.Domain.Events;

public record TodoDeletedEvent : DomainEvent
{
  public TodoDeletedEvent()
  {
    IsDeleted = true;
  }
}
