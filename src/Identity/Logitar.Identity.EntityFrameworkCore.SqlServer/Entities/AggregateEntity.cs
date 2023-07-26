using Logitar.EventSourcing;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Actors;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Constants;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;

public abstract record AggregateEntity
{
  protected AggregateEntity()
  {
  }
  protected AggregateEntity(DomainEvent change, ActorEntity actor)
  {
    AggregateId = change.AggregateId.Value;

    CreatedById = change.ActorId ?? Actor.DefaultId;
    CreatedBy = actor.Serialize();
    CreatedOn = change.OccurredOn;

    Update(change, actor);
  }

  public string AggregateId { get; private set; } = string.Empty;

  public string CreatedById { get; private set; } = string.Empty;
  public string CreatedBy { get; private set; } = string.Empty;
  public DateTime CreatedOn { get; private set; }

  public string UpdatedById { get; private set; } = string.Empty;
  public string UpdatedBy { get; private set; } = string.Empty;
  public DateTime UpdatedOn { get; private set; }

  public long Version { get; private set; }

  public virtual void SetActor(string id, string json)
  {
    if (CreatedById == id)
    {
      CreatedBy = json;
    }

    if (UpdatedById == id)
    {
      UpdatedBy = json;
    }
  }

  protected void SetVersion(DomainEvent change) => Version = change.Version;

  protected void Update(DomainEvent change, ActorEntity actor)
  {
    UpdatedById = change.ActorId ?? Actor.DefaultId;
    UpdatedBy = actor.Serialize();
    UpdatedOn = change.OccurredOn;

    SetVersion(change);
  }
}
