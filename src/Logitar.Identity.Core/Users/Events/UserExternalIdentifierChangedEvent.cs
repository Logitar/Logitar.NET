using Logitar.EventSourcing;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when an external identifier of an <see cref="UserAggregate"/> is changed.
/// </summary>
public record UserExternalIdentifierChangedEvent : DomainEvent
{
  /// <summary>
  /// Gets or sets the key of the external identifier.
  /// </summary>
  public string Key { get; init; } = string.Empty;
  /// <summary>
  /// Gets or sets the value of the external identifier.
  /// <br />When set to null, the external identifier will be removed.
  /// </summary>
  public string? Value { get; init; }
}
