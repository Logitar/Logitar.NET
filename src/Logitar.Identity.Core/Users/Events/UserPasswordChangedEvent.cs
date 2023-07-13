using Logitar.EventSourcing;
using Logitar.Security.Cryptography;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when the password of an <see cref="UserAggregate"/> is changed.
/// </summary>
public record UserPasswordChangedEvent : DomainEvent
{
  /// <summary>
  /// Gets or sets the password of the user.
  /// </summary>
  public Pbkdf2 Password { get; init; } = new(string.Empty);
}
