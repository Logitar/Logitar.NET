using Logitar.EventSourcing;
using Logitar.Identity.EntityFrameworkCore.Relational.Actors;
using Logitar.Identity.EntityFrameworkCore.Relational.Constants;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Entities;

public record ExternalIdentifierEntity
{
  public ExternalIdentifierEntity(DomainEvent change, ActorEntity actor, UserEntity user, string key, string value)
  {
    Id = Guid.NewGuid();

    User = user;
    UserId = user.UserId;

    TenantId = user.TenantId;
    Key = key;
    Value = value;

    CreatedById = change.ActorId ?? Actor.DefaultId;
    CreatedBy = actor.Serialize();
    CreatedOn = change.OccurredOn.ToUniversalTime();

    UpdatedById = change.ActorId ?? Actor.DefaultId;
    UpdatedBy = actor.Serialize();
    UpdatedOn = change.OccurredOn.ToUniversalTime();

    Version = 1;
  }

  private ExternalIdentifierEntity()
  {
  }

  public int ExternalIdentifierId { get; private set; }

  public Guid Id { get; private set; }

  public UserEntity? User { get; private set; }
  public int UserId { get; private set; }

  public string? TenantId { get; private set; }
  public string Key { get; private set; } = string.Empty;
  public string Value { get; private set; } = string.Empty;
  public string ValueNormalized
  {
    get => Value.ToUpper();
    private set { }
  }

  public string CreatedById { get; private set; } = string.Empty;
  public string CreatedBy { get; private set; } = string.Empty;
  public DateTime CreatedOn { get; private set; }

  public string UpdatedById { get; private set; } = string.Empty;
  public string UpdatedBy { get; private set; } = string.Empty;
  public DateTime UpdatedOn { get; private set; }

  public long Version { get; private set; }

  public void SetActor(string id, string json)
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

  public void Update(DomainEvent change, ActorEntity actor, string value)
  {
    Value = value;

    UpdatedById = change.ActorId ?? Actor.DefaultId;
    UpdatedBy = actor.Serialize();
    UpdatedOn = change.OccurredOn.ToUniversalTime();

    Version++;
  }
}
