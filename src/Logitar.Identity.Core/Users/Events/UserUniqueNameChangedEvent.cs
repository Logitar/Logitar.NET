using Logitar.EventSourcing;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when the unique name of a <see cref="UserAggregate"/> is modified.
/// </summary>
public record UserUniqueNameChangedEvent : DomainEvent
{
  /// <summary>
  /// Gets or sets the unique name of the user.
  /// </summary>
  public string UniqueName { get; init; } = string.Empty;
}
