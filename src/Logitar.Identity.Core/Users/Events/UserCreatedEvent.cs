using Logitar.EventSourcing;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when an <see cref="UserAggregate"/> is created.
/// </summary>
public record UserCreatedEvent : DomainEvent
{
  /// <summary>
  /// Gets or sets the identifier of the tenant in which the role belongs.
  /// </summary>
  public string? TenantId { get; init; }

  /// <summary>
  /// Gets or sets the unique name of the role.
  /// </summary>
  public string UniqueName { get; init; } = string.Empty;
}
