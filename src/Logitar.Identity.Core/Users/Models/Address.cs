namespace Logitar.Identity.Core.Users.Models;

/// <summary>
/// Represents the read model for postal addresses.
/// </summary>
public record Address : Contact
{
  /// <summary>
  /// Gets or sets the street address. It may contain multiple lines, separated by newlines.
  /// </summary>
  public string Street { get; set; } = string.Empty;
  /// <summary>
  /// Gets or sets the locality of the postal address.
  /// </summary>
  public string Locality { get; set; } = string.Empty;
  /// <summary>
  /// Gets or sets the country of the postal address.
  /// </summary>
  public string Country { get; set; } = string.Empty;
  /// <summary>
  /// Gets or sets the region of the postal address.
  /// </summary>
  public string? Region { get; set; }
  /// <summary>
  /// Gets or sets the postal code of the postal address.
  /// </summary>
  public string? PostalCode { get; set; }
  /// <summary>
  /// Gets or sets the formatted text of the postal address.
  /// </summary>
  public string Formatted { get; set; } = string.Empty;
}
