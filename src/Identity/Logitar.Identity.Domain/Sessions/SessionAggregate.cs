using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Sessions.Events;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.Domain.Validators;

namespace Logitar.Identity.Domain.Sessions;

public class SessionAggregate : AggregateRoot
{
  private readonly Dictionary<string, string> _customAttributes = new();
  private readonly CustomAttributeValidator _customAttributeValidator = new();

  private Password? _secret = null;

  public SessionAggregate(AggregateId id) : base(id)
  {
  }

  public SessionAggregate(UserAggregate user, Password? secret = null, ActorId actorId = default)
    : base()
  {
    ApplyChange(new SessionCreatedEvent
    {
      ActorId = actorId,
      UserId = user.Id,
      Secret = secret
    });
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

  public void Delete(ActorId actorId = default) => ApplyChange(new SessionDeletedEvent(actorId));

  public void RemoveCustomAttribute(string key)
  {
    key = key.Trim();
    if (_customAttributes.ContainsKey(key))
    {
      SessionUpdatedEvent updated = GetLatestEvent<SessionUpdatedEvent>();
      updated.CustomAttributes[key] = null;
      Apply(updated);
    }
  }

  public void Renew(string currentSecret, Password newSecret, ActorId actorId = default)
  {
    if (!IsActive)
    {
      throw new SessionIsNotActiveException(this);
    }
    else if (_secret?.IsMatch(currentSecret) != true)
    {
      throw new IncorrectSessionSecretException(this, currentSecret);
    }

    ApplyChange(new SessionRenewedEvent(newSecret)
    {
      ActorId = actorId
    });
  }
  protected virtual void Apply(SessionRenewedEvent renewed) => _secret = renewed.Secret;

  public void SetCustomAttribute(string key, string value)
  {
    key = key.Trim();
    value = value.Trim();
    _customAttributeValidator.ValidateAndThrow(key, value);

    if (!_customAttributes.TryGetValue(key, out string? existingValue) || value != existingValue)
    {
      SessionUpdatedEvent updated = GetLatestEvent<SessionUpdatedEvent>();
      updated.CustomAttributes[key] = value;
      Apply(updated);
    }
  }

  public void SignOut(ActorId actorId = default)
  {
    if (IsActive)
    {
      ApplyChange(new SessionSignedOutEvent(actorId));
    }
  }
  protected virtual void Apply(SessionSignedOutEvent _) => IsActive = false;

  public void Update(ActorId actorId = default)
  {
    foreach (DomainEvent change in Changes)
    {
      if (change is SessionUpdatedEvent)
      {
        change.ActorId = actorId;

        if (change.Version == Version)
        {
          UpdatedBy = actorId;
        }
      }
    }
  }

  protected virtual void Apply(SessionUpdatedEvent updated)
  {
    foreach (KeyValuePair<string, string?> customAttributes in updated.CustomAttributes)
    {
      if (customAttributes.Value == null)
      {
        _customAttributes.Remove(customAttributes.Key);
      }
      else
      {
        _customAttributes[customAttributes.Key] = customAttributes.Value;
      }
    }
  }
  protected virtual T GetLatestEvent<T>() where T : DomainEvent, new()
  {
    T? change = Changes.LastOrDefault(change => change is T) as T;
    if (change == null)
    {
      change = new();
      ApplyChange(change);
    }

    return change;
  }
}
