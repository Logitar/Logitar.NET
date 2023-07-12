namespace Logitar.EventSourcing;

public class PersonAggregate : AggregateRoot
{
  public PersonAggregate(AggregateId id) : base(id)
  {
  }

  public PersonAggregate(string fullName, string? actorId = null, DateTime? occurredOn = null) : base()
  {
    ApplyChange(new PersonCreatedEvent(fullName), actorId, occurredOn);
  }
  protected virtual void Apply(PersonCreatedEvent e) => FullName = e.FullName;

  public string FullName { get; private set; } = string.Empty;

  public void Delete() => ApplyChange(new PersonDeletedChangedEvent(DeleteAction.Delete));
  public void Undelete() => ApplyChange(new PersonDeletedChangedEvent(DeleteAction.Undelete));

  public void Dispatch(DomainEvent e)
  {
    MethodInfo dispatch = typeof(AggregateRoot)
      .GetMethod("Dispatch", BindingFlags.Instance | BindingFlags.NonPublic, new[] { typeof(DomainEvent) })
      ?? throw new InvalidOperationException("The Dispatch method could not be found.");

    try
    {
      dispatch.Invoke(this, new[] { e });
    }
    catch (Exception exception)
    {
      if (exception.InnerException == null)
      {
        throw;
      }

      throw exception.InnerException;
    }
  }
}
