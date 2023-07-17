using Logitar.EventSourcing;

namespace Logitar.Identity.Core.Sessions.Events;

/// <summary>
/// The event raised when a <see cref="SessionAggregate"/> is modified.
/// </summary>
public record SessionModifiedEvent : DomainEvent
{
  /// <summary>
  /// Gets or sets the custom attribute modifications of the session.
  /// <br />If the value is null, the custom attribute will be removed.
  /// <br />Otherwise, the custom attribute will be added or replaced.
  /// </summary>
  public Dictionary<string, string?> CustomAttributes { get; init; } = new();
}
