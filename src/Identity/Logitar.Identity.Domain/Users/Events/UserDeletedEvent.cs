using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

public record UserDeletedEvent : DomainEvent, INotification
{
  public UserDeletedEvent()
  {
    DeleteAction = DeleteAction.Delete;
  }
}
