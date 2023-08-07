using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Sessions.Events;

public record SessionDeletedEvent : DomainEvent
{
  public SessionDeletedEvent(ActorId actorId = default)
  {
    ActorId = actorId;
    IsDeleted = true;
  }
}
