using Logitar.EventSourcing;

namespace Logitar.Identity.Core.Roles.Events;

/// <summary>
/// The event raised when a <see cref="RoleAggregate"/> is modified.
/// </summary>
public record RoleModifiedEvent : DomainEvent
{
  /// <summary>
  /// Gets or sets the display name of the role.
  /// </summary>
  public Modification<string> DisplayName { get; set; }
  /// <summary>
  /// Gets or sets the description of the role.
  /// </summary>
  public Modification<string> Description { get; set; }

  /// <summary>
  /// Gets or sets the custom attribute modifications of the role.
  /// <br />If the value is null, the custom attribute will be removed.
  /// <br />Otherwise, the custom attribute will be added or replaced.
  /// </summary>
  public Dictionary<string, string?> CustomAttributes { get; init; } = new();
}
