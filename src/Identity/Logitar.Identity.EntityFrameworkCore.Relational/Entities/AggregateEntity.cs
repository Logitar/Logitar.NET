using Logitar.EventSourcing;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Entities;

public abstract record AggregateEntity
{
  protected AggregateEntity()
  {
  }

  protected AggregateEntity(DomainEvent change)
  {
    AggregateId = change.AggregateId.Value;

    CreatedBy = change.ActorId.Value;
    CreatedOn = change.OccurredOn.ToUniversalTime();

    Update(change);
  }

  public string AggregateId { get; private set; } = EventSourcing.AggregateId.NewId().Value;

  public string CreatedBy { get; private set; } = ActorId.DefaultValue;
  public DateTime CreatedOn { get; private set; }

  public string UpdatedBy { get; private set; } = ActorId.DefaultValue;
  public DateTime UpdatedOn { get; private set; }

  public long Version { get; private set; }

  public virtual void Update(DomainEvent change)
  {
    UpdatedBy = change.ActorId.Value;
    UpdatedOn = change.OccurredOn.ToUniversalTime();

    Version = change.Version;
  }
}
