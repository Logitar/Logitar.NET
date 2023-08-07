using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Sessions.Events;

public record SessionSignedOutEvent : DomainEvent
{
  public SessionSignedOutEvent(ActorId actorId = default)
  {
    ActorId = actorId;
  }
}
