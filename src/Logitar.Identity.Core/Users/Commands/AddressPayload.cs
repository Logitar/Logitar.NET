namespace Logitar.Identity.Core.Users.Commands;

/// <summary>
/// The postal address input data.
/// </summary>
public record AddressPayload : ContactPayload
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
}
