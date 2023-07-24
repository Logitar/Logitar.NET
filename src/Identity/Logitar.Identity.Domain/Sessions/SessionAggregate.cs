using Logitar.EventSourcing;
using Logitar.Identity.Domain.Sessions.Events;
using Logitar.Identity.Domain.Users;
using Logitar.Security;

namespace Logitar.Identity.Domain.Sessions;

public class SessionAggregate : AggregateRoot
{
  public const int SecretLength = 256 / 8;

  private Pbkdf2? _secret = null;

  public SessionAggregate(AggregateId id) : base(id)
  {
  }

  public SessionAggregate(UserAggregate user, bool isPersistent = false, DateTime? createdOn = null)
  {
    Secret = isPersistent ? RandomNumberGenerator.GetBytes(SecretLength) : null;

    ApplyChange(new SessionCreatedEvent
    {
      UserId = user.Id,
      Secret = Secret == null ? null : new Pbkdf2(Secret)
    }, actorId: user.Id.Value, occurredOn: createdOn);
  }
  protected virtual void Apply(SessionCreatedEvent created)
  {
    _secret = created.Secret;

    IsActive = true;
  }

  public bool IsPersistent => _secret != null;

  public bool IsActive { get; private set; }

  public byte[]? Secret { get; private set; }
}
