using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Sessions.Events;

public record SessionDeletedEvent : DomainEvent, INotification
{
  public SessionDeletedEvent()
  {
    DeleteAction = DeleteAction.Delete;
  }
}
