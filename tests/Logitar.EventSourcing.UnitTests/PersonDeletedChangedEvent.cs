namespace Logitar.EventSourcing;

public record PersonDeletedChangedEvent : DomainEvent
{
  public PersonDeletedChangedEvent(DeleteAction deleteAction)
  {
    DeleteAction = deleteAction;
  }
}
