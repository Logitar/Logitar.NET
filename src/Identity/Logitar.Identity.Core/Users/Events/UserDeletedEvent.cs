using Logitar.EventSourcing;

namespace Logitar.Identity.Core.Users.Events;

public record UserDeletedEvent : DomainEvent
{
  public UserDeletedEvent()
  {
    DeleteAction = DeleteAction.Delete;
  }
}
