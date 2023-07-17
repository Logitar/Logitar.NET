using Logitar.EventSourcing;
using Logitar.Identity.Core.Users.Contact;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when the postal address of an <see cref="UserAggregate"/> is changed.
/// </summary>
public record UserAddressChangedEvent : DomainEvent
{
  /// <summary>
  /// Gets or sets the postal address of the user.
  /// </summary>
  public ReadOnlyAddress? Address { get; init; }
}
