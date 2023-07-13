using Logitar.EventSourcing;

namespace Logitar.Identity.Core.ApiKeys.Events;

/// <summary>
/// The event raised when an <see cref="ApiKeyAggregate"/> is modified.
/// </summary>
public record ApiKeyModifiedEvent : DomainEvent
{
  /// <summary>
  /// Gets or sets the display name of the API key.
  /// </summary>
  public Modification<string> Title { get; set; }
  /// <summary>
  /// Gets or sets the description of the API key.
  /// </summary>
  public Modification<string> Description { get; set; }
  /// <summary>
  /// Gets or sets the expiration date and time of the API key.
  /// </summary>
  public Modification<DateTime?> ExpiresOn { get; set; }

  /// <summary>
  /// Gets or sets the custom attribute modifications of the API key.
  /// <br />If the value is null, the custom attribute will be removed.
  /// <br />Otherwise, the custom attribute will be added or replaced.
  /// </summary>
  public Dictionary<string, string?> CustomAttributes { get; init; } = new();
  /// <summary>
  /// Gets or sets the role modifications of the API key.
  /// <br />The key of the dictionary corresponds to the identifier of the role.
  /// <br />If the value is true, then the role will be added.
  /// <br />Otherwise, the role will be removed.
  /// </summary>
  public Dictionary<string, bool> Roles { get; init; } = new();
}
