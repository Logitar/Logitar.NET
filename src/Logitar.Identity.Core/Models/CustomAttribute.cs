namespace Logitar.Identity.Core.Models;

/// <summary>
/// Represents a custom attribute.
/// </summary>
public record CustomAttribute
{
  /// <summary>
  /// Gets or sets the key of the custom attribute.
  /// </summary>
  public string Key { get; set; } = string.Empty;
  /// <summary>
  /// Gets or sets the value of the custom attribute.
  /// </summary>
  public string Value { get; set; } = string.Empty;
}
