using Logitar.EventSourcing;
using Logitar.Identity.Core.Users.Contact;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when the phone of an <see cref="UserAggregate"/> is changed.
/// </summary>
public record UserPhoneChangedEvent : DomainEvent
{
  /// <summary>
  /// Gets or sets the email of the user.
  /// </summary>
  public ReadOnlyPhone? Phone { get; init; }
}
