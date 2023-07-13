using Logitar.EventSourcing;

namespace Logitar.Identity.Core.Roles.Events;

/// <summary>
/// The event raised when the unique name of a <see cref="RoleAggregate"/> is modified.
/// </summary>
public record RoleUniqueNameChangedEvent : DomainEvent
{
  /// <summary>
  /// Gets or sets the unique name of the role.
  /// </summary>
  public string UniqueName { get; init; } = string.Empty;
}
