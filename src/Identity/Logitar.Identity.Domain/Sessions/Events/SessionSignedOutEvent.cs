using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Sessions.Events;

public record SessionSignedOutEvent : DomainEvent, INotification
{
  public SessionSignedOutEvent(ActorId actorId = default)
  {
    ActorId = actorId;
  }
}
