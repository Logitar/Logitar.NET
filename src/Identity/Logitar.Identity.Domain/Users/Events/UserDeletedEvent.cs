using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Users.Events;

public record UserDeletedEvent : DomainEvent
{
  public UserDeletedEvent()
  {
    DeleteAction = DeleteAction.Delete;
  }
}
