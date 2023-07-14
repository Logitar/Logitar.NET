using Logitar.EventSourcing;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when the status of an <see cref="UserAggregate"/> is changed.
/// </summary>
public record UserStatusChangedEvent : DomainEvent
{
  /// <summary>
  /// Gets or sets a value indicating whether or not the user is disabled.
  /// </summary>
  public bool IsDisabled { get; init; }
}
