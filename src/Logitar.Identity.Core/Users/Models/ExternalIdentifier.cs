using Logitar.Identity.Core.Models;

namespace Logitar.Identity.Core.Users.Models;

/// <summary>
/// Represents the read model for external identifiers.
/// </summary>
public record ExternalIdentifier
{
  /// <summary>
  /// Gets or sets the identifier of the external identifier.
  /// </summary>
  public string Id { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the key of the external identifier.
  /// </summary>
  public string Key { get; set; } = string.Empty;
  /// <summary>
  /// Gets or sets the value of the external identifier.
  /// </summary>
  public string Value { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the actor who created the external identifier.
  /// </summary>
  public Actor CreatedBy { get; set; } = new();
  /// <summary>
  /// Gets or sets the date and time when the external identifier was created.
  /// </summary>
  public DateTime CreatedOn { get; set; }

  /// <summary>
  /// Gets or sets the actor who updated the external identifier lastly.
  /// </summary>
  public Actor UpdatedBy { get; set; } = new();
  /// <summary>
  /// Gets or sets the latest date and time when the external identifier was updated.
  /// </summary>
  public DateTime UpdatedOn { get; set; }

  /// <summary>
  /// Gets or sets the version of the external identifier.
  /// </summary>
  public long Version { get; set; }
}
