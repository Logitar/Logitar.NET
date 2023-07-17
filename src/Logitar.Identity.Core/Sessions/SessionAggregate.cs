using FluentValidation;
using Logitar.EventSourcing;
using Logitar.Identity.Core.Sessions.Events;
using Logitar.Identity.Core.Users;
using Logitar.Identity.Core.Validators;
using Logitar.Security.Cryptography;

namespace Logitar.Identity.Core.Sessions;

/// <summary>
/// TODO(fpion): document
/// </summary>
public class SessionAggregate : AggregateRoot
{
  /// <summary>
  /// The length of persistence token secrets (256 bits).
  /// </summary>
  private const int SecretLength = 256 / 8;

  /// <summary>
  /// The custom attributes of the session.
  /// </summary>
  private readonly Dictionary<string, string> _customAttributes = new();

  /// <summary>
  /// The persistence token of the session.
  /// </summary>
  private Pbkdf2? _secret = null;

  /// <summary>
  /// Initializes a new instance of the <see cref="SessionAggregate"/> class.
  /// </summary>
  /// <param name="id">The identifier of the session.</param>
  public SessionAggregate(AggregateId id) : base(id)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="SessionAggregate"/> class.
  /// </summary>
  /// <param name="user">The user to whom the session belongs to.</param>
  /// <param name="isPersistent">A value indicating whether or not a persistence token will be generated.</param>
  /// <param name="id">The identifier of the session.</param>
  public SessionAggregate(UserAggregate user, bool isPersistent = false, AggregateId? id = null) : base(id)
  {
    if (isPersistent)
    {
      Secret = RandomNumberGenerator.GetBytes(SecretLength);
    }

    ApplyChange(new SessionCreatedEvent()
    {
      UserId = user.Id,
      Secret = Secret == null ? null : new Pbkdf2(Secret)
    });
  }
  /// <summary>
  /// Applies the specified event to the aggregate.
  /// </summary>
  /// <param name="e">The event to apply.</param>
  protected virtual void Apply(SessionCreatedEvent e)
  {
    _secret = e.Secret;

    UserId = e.UserId;

    IsActive = true;
  }

  /// <summary>
  /// Gets or sets the identifier of the user to whom the session belongs to.
  /// </summary>
  public AggregateId UserId { get; private set; }

  /// <summary>
  /// Gets a value indicating whether or not the session is persistent.
  /// <br />A persistent session has a persistent token that allow it to be renewed.
  /// </summary>
  public bool IsPersistent => _secret != null;

  /// <summary>
  /// Gets or sets a value indicating whether the session is active.
  /// <br />An inactive session is signed-out. It cannot be renewed nor signed-out again.
  /// </summary>
  public bool IsActive { get; private set; }

  /// <summary>
  /// Gets the custom attributes of the session.
  /// </summary>
  public IReadOnlyDictionary<string, string> CustomAttributes => _customAttributes.AsReadOnly();

  /// <summary>
  /// Gets or sets the secret bytes of the persistence token.
  /// </summary>
  public byte[]? Secret { get; private set; }

  /// <summary>
  /// Deletes the session.
  /// </summary>
  public void Delete() => ApplyChange(new SessionDeletedEvent());

  /// <summary>
  /// Removes a custom attribute on the session.
  /// </summary>
  /// <param name="key">The key of the custom attribute.</param>
  public void RemoveCustomAttribute(string key)
  {
    key = key.Trim();
    if (_customAttributes.ContainsKey(key))
    {
      SessionModifiedEvent e = GetLatestModifiedEvent();
      e.CustomAttributes[key] = null;
      Apply(e);
    }
  }
  /// <summary>
  /// Sets a custom attribute on the session.
  /// </summary>
  /// <param name="key">The key of the custom attribute.</param>
  /// <param name="value">The value of the custom attribute.</param>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public void SetCustomAttribute(string key, string value)
  {
    key = key.Trim();
    value = value.Trim();
    CustomAttributeValidator.Instance.ValidateAndThrow(key, value);

    if (!_customAttributes.TryGetValue(key, out string? existingValue) || value != existingValue)
    {
      SessionModifiedEvent e = GetLatestModifiedEvent();
      e.CustomAttributes[key] = value;
      Apply(e);
    }
  }

  /// <summary>
  /// Renew the session.
  /// </summary>
  /// <param name="secret">The secret bytes to validate.</param>
  /// <exception cref="SessionIsNotActiveException">The session was not active.</exception>
  /// <exception cref="InvalidCredentialsException">The specified secret was not valid.</exception>
  public void Renew(byte[] secret)
  {
    if (!IsActive)
    {
      throw new SessionIsNotActiveException(this);
    }
    else if (_secret?.IsMatch(secret) != true)
    {
      StringBuilder message = new();
      message.AppendLine("The session secret was not a match.");
      message.Append("Session: ").Append(this).AppendLine();
      message.Append("Secret: ").Append(Convert.ToBase64String(secret)).AppendLine();
      throw new InvalidCredentialsException(message.ToString());
    }

    Secret = RandomNumberGenerator.GetBytes(SecretLength);

    ApplyChange(new SessionRenewedEvent
    {
      Secret = new Pbkdf2(Secret)
    });
  }
  /// <summary>
  /// Applies the specified event to the aggregate.
  /// </summary>
  /// <param name="e">The event to apply.</param>
  protected virtual void Apply(SessionRenewedEvent e) => _secret = e.Secret;

  /// <summary>
  /// Signs-out the session.
  /// </summary>
  public void SignOut()
  {
    if (IsActive)
    {
      ApplyChange(new SessionSignedOutEvent());
    }
  }
  /// <summary>
  /// Applies the specified event to the aggregate.
  /// </summary>
  /// <param name="e">The event to apply.</param>
  protected virtual void Apply(SessionSignedOutEvent e) => IsActive = false;

  /// <summary>
  /// Applies the specified event to the aggregate.
  /// </summary>
  /// <param name="e">The event to apply.</param>
  protected virtual void Apply(SessionModifiedEvent e)
  {
    foreach (KeyValuePair<string, string?> customAttribute in e.CustomAttributes)
    {
      if (customAttribute.Value == null)
      {
        _customAttributes.Remove(customAttribute.Key);
      }
      else
      {
        _customAttributes[customAttribute.Key] = customAttribute.Value;
      }
    }
  }
  /// <summary>
  /// Finds or applies the latest session modification event.
  /// </summary>
  /// <returns>The latest user modification event.</returns>
  private SessionModifiedEvent GetLatestModifiedEvent()
  {
    SessionModifiedEvent? e = Changes.LastOrDefault(e => e is SessionModifiedEvent) as SessionModifiedEvent;
    if (e == null)
    {
      e = new();
      ApplyChange(e);
    }

    return e;
  }
}
