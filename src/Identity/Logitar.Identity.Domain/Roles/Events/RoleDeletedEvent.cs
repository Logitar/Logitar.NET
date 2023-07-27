using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Roles.Events;

public record RoleDeletedEvent : DomainEvent, INotification
{
  public RoleDeletedEvent()
  {
    DeleteAction = DeleteAction.Delete;
  }
}
