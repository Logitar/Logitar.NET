using Logitar.Identity.Domain.Sessions.Events;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Actors;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Constants;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;

public record SessionEntity : AggregateEntity, ICustomAttributesProvider
{
  public SessionEntity(SessionCreatedEvent created, ActorEntity actor, UserEntity user) : base(created, actor)
  {
    UserId = user.UserId;
    User = user;

    Secret = created.Secret?.Encode();

    IsActive = true;
  }

  private SessionEntity() : base()
  {
  }

  public int SessionId { get; private set; }

  public int UserId { get; private set; }
  public UserEntity? User { get; private set; }

  public string? Secret { get; private set; }
  public bool IsPersistent
  {
    get => Secret != null;
    private set { }
  }

  public bool IsActive { get; private set; }
  public string? SignedOutById { get; private set; }
  public string? SignedOutBy { get; private set; }
  public DateTime? SignedOutOn { get; private set; }

  public string? CustomAttributes { get; private set; }

  public void Renew(SessionRenewedEvent renewed, ActorEntity actor)
  {
    Update(renewed, actor);

    Secret = renewed.Secret.Encode();
  }

  public override void SetActor(string id, string json)
  {
    base.SetActor(id, json);

    if (SignedOutById == id)
    {
      SignedOutBy = json;
    }
  }

  public void SignOut(SessionSignedOutEvent signedOut, ActorEntity actor)
  {
    SetVersion(signedOut);

    IsActive = false;
    SignedOutById = signedOut.ActorId ?? Actor.DefaultId;
    SignedOutBy = actor.Serialize();
    SignedOutOn = signedOut.OccurredOn;
  }

  public void Update(SessionUpdatedEvent updated, ActorEntity actor)
  {
    Update(updated, actor);

    CustomAttributes = this.UpdateCustomAttributes(updated.CustomAttributes);
  }
}
