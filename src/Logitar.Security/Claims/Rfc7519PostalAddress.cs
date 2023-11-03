namespace Logitar.Security.Claims;

/// <summary>
/// Defines an object to represent a postal address in an address claim.
/// Reference: https://openid.net/specs/openid-connect-core-1_0.html#AddressClaim
/// </summary>
public record Rfc7519PostalAddress
{
  /// <summary>
  /// Gets or sets the full mailing address, formatted for display or use on a mailing label. This
  /// field MAY contain multiple lines, separated by newlines. Newlines can be represented either as
  /// a carriage return/line feed pair ("\r\n") or as a single line feed character ("\n").
  /// </summary>
  [JsonPropertyName("formatted")]
  public string Formatted { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the full street address component, which MAY include house number, street name,
  /// Post Office Box, and multi-line extended street address information. This field MAY contain
  /// multiple lines, separated by newlines. Newlines can be represented either as a carriage
  /// return/line feed pair ("\r\n") or as a single line feed character ("\n").
  /// </summary>
  [JsonPropertyName("street_address")]
  public string StreetAddress { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the city or locality component.
  /// </summary>
  [JsonPropertyName("locality")]
  public string Locality { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the state, province, prefecture, or region component.
  /// </summary>
  [JsonPropertyName("region")]
  public string? Region { get; set; }

  /// <summary>
  /// Gets or sets the zip code or postal code component.
  /// </summary>
  [JsonPropertyName("postal_code")]
  public string? PostalCode { get; set; }

  /// <summary>
  /// Gets or sets the country name component.
  /// </summary>
  [JsonPropertyName("country")]
  public string Country { get; set; } = string.Empty;

  /// <summary>
  /// Serializes the current instance of a postal address to JSON to be used within an address claim.
  /// </summary>
  /// <returns>The postal address serialized as JSON.</returns>
  public string Serialize() => JsonSerializer.Serialize(this);
}
