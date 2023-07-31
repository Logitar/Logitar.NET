using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Sessions.Events;
using Logitar.Identity.Domain.Users;
using Logitar.Security;

namespace Logitar.Identity.Domain.Sessions;

public class SessionAggregate : AggregateRoot
{
  public const int SecretLength = 256 / 8;

  private Password? _secret = null;

  public SessionAggregate(AggregateId id) : base(id)
  {
  }

  public SessionAggregate(UserAggregate user, bool isPersistent = false, DateTime? createdOn = null) : base()
  {
    byte[]? secret = null;
    ApplyChange(new SessionCreatedEvent
    {
      UserId = user.Id,
      Secret = isPersistent ? PasswordHelper.Generate(SecretLength, out secret) : null
    }, actorId: user.Id.Value, occurredOn: createdOn);
    Secret = secret;
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

  public void Delete() => ApplyChange(new SessionDeletedEvent());

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

    ApplyChange(new SessionRenewedEvent
    {
      Secret = PasswordHelper.Generate(SecretLength, out byte[] newSecret)
    }, actorId: UserId.Value);
    Secret = newSecret;
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
