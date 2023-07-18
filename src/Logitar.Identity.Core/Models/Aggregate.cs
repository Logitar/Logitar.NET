namespace Logitar.Identity.Core.Models;

/// <summary>
/// Represents the base model for aggregates.
/// </summary>
public abstract record Aggregate
{
  /// <summary>
  /// Gets or sets the identifier of the aggregate.
  /// </summary>
  public string Id { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the actor who created the aggregate.
  /// </summary>
  public Actor CreatedBy { get; set; } = new();
  /// <summary>
  /// Gets or sets the date and time when the aggregate was created.
  /// </summary>
  public DateTime CreatedOn { get; set; }

  /// <summary>
  /// Gets or sets the actor who updated the aggregate lastly.
  /// </summary>
  public Actor UpdatedBy { get; set; } = new();
  /// <summary>
  /// Gets or sets the latest date and time when the aggregate was updated.
  /// </summary>
  public DateTime UpdatedOn { get; set; }

  /// <summary>
  /// Gets or sets the version of the aggregate.
  /// </summary>
  public long Version { get; set; }
}
