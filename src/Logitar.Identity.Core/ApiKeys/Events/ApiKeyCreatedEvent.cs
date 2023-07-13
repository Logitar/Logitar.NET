using Logitar.EventSourcing;
using Logitar.Security.Cryptography;

namespace Logitar.Identity.Core.ApiKeys.Events;

/// <summary>
/// The event raised when an <see cref="ApiKeyAggregate"/> is created.
/// </summary>
public record ApiKeyCreatedEvent : DomainEvent
{
  /// <summary>
  /// Gets or sets the identifier of the tenant in which the API key belongs.
  /// </summary>
  public string? TenantId { get; init; }

  /// <summary>
  /// Gets or sets the secret of the API key.
  /// </summary>
  public Pbkdf2 Secret { get; init; } = new(string.Empty);

  /// <summary>
  /// Gets or sets the title of the API key.
  /// </summary>
  public string Title { get; init; } = string.Empty;
}
