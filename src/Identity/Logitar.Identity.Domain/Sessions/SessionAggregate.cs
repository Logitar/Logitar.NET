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

    UserId = created.UserId;

    IsActive = true;
  }

  public AggregateId UserId { get; private set; }

  public bool IsPersistent => _secret != null;

  public bool IsActive { get; private set; }

  public byte[]? Secret { get; private set; }

  public void Renew(byte[] secret)
  {
    if (!IsActive)
    {
      throw new SessionIsNotActiveException(this);
    }

    if (_secret?.IsMatch(secret) != true)
    {
      StringBuilder message = new();
      message.AppendLine("The specified secret does not match the session.");
      message.Append("Session: ").AppendLine(ToString());
      message.Append("Secret: ").AppendLine(Convert.ToBase64String(secret));
      throw new InvalidCredentialsException(message.ToString());
    }

    Secret = RandomNumberGenerator.GetBytes(SecretLength);

    ApplyChange(new SessionRenewedEvent
    {
      Secret = new Pbkdf2(Secret)
    }, actorId: UserId.Value);
  }
  protected virtual void Apply(SessionRenewedEvent renewed) => _secret = renewed.Secret;

  public void SignOut()
  {
    if (IsActive)
    {
      ApplyChange(new SessionSignedOutEvent());
    }
  }
  protected virtual void Apply(SessionSignedOutEvent signedOut) => IsActive = false;
}
