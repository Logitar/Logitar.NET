using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

public record UserDeletedEvent : DomainEvent, INotification
{
  public UserDeletedEvent(ActorId actorId = default)
  {
    ActorId = actorId;
    IsDeleted = true;
  }
}
