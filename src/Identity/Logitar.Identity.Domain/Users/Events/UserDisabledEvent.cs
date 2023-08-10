using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

public record UserDisabledEvent : DomainEvent, INotification
{
  public UserDisabledEvent(ActorId actorId = default)
  {
    ActorId = actorId;
  }
}
