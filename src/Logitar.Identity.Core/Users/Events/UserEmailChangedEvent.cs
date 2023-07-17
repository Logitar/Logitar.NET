using Logitar.EventSourcing;
using Logitar.Identity.Core.Users.Contact;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when the email address of an <see cref="UserAggregate"/> is changed.
/// </summary>
public record UserEmailChangedEvent : DomainEvent
{
  /// <summary>
  /// Gets or sets the email address of the user.
  /// </summary>
  public ReadOnlyEmail? Email { get; init; }
}
