namespace Logitar.EventSourcing;

internal record PersonDeletedChangedEvent : DomainEvent
{
  public PersonDeletedChangedEvent(DeleteAction deleteAction)
  {
    DeleteAction = deleteAction;
  }
}
