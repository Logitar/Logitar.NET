using Logitar.EventSourcing;

namespace Logitar.Identity.Core.ApiKeys.Events;

/// <summary>
/// The event raised when a <see cref="ApiKeyAggregate"/> is created.
/// </summary>
public record ApiKeyCreatedEvent : DomainEvent
{
  /// <summary>
  /// Gets or sets the identifier of the tenant in which the API key belongs.
  /// </summary>
  public string? TenantId { get; init; }

  /// <summary>
  /// Gets or sets the title of the API key.
  /// </summary>
  public string Title { get; init; } = string.Empty;
}
