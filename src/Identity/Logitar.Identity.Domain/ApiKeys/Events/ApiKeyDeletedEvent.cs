using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.ApiKeys.Events;

public record ApiKeyDeletedEvent : DomainEvent, INotification
{
  public ApiKeyDeletedEvent()
  {
    DeleteAction = DeleteAction.Delete;
  }
}
