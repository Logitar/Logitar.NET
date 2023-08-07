using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Users.Events;

public record UserDeletedEvent : DomainEvent
{
  public UserDeletedEvent(ActorId actorId = default)
  {
    ActorId = actorId;
    IsDeleted = true;
  }
}
