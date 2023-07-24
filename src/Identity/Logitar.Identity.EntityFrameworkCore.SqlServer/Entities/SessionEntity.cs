using Logitar.Identity.Domain.Sessions.Events;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Actors;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;

public record SessionEntity : AggregateEntity
{
  public SessionEntity(SessionCreatedEvent created, ActorEntity actor, UserEntity user) : base(created, actor)
  {
    UserId = user.UserId;
    User = user;

    Secret = created.Secret?.ToString();

    IsActive = true;
  }

  private SessionEntity() : base()
  {
  }

  public int SessionId { get; private set; }

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

  public int UserId { get; private set; }
  public UserEntity? User { get; private set; }

  public void Renew(SessionRenewedEvent renewed, ActorEntity actor)
  {
    Update(renewed, actor);

    Secret = renewed.Secret.ToString();
  }
}
