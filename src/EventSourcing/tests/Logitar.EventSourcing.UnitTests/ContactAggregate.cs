namespace Logitar.EventSourcing;

public class ContactAggregate : AggregateRoot
{
  public ContactAggregate(AggregateId id) : base(id)
  {
  }

  public ContactAggregate(PersonAggregate person, ContactType type, string value,
    string? actorId = null, DateTime? occurredOn = null) : base()
  {
    ApplyChange(new ContactCreatedEvent(person.Id, type, value), actorId, occurredOn);
  }
  protected virtual void Apply(ContactCreatedEvent e)
  {
    PersonId = e.PersonId;

    Type = e.Type;
    Value = e.Value;
  }

  public AggregateId PersonId { get; private set; }

  public ContactType Type { get; private set; }
  public string Value { get; private set; } = string.Empty;
}
