namespace Logitar.Identity.Core.Users.Models;

/// <summary>
/// Represents the read model for phone numbers.
/// </summary>
public record Phone : Contact
{
  /// <summary>
  /// Gets or sets the two-letter country code of the phone.
  /// <br />See <see href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2"/> for more detail.
  /// </summary>
  public string? CountryCode { get; set; }
  /// <summary>
  /// Gets or sets the number of the phone.
  /// </summary>
  public string Number { get; set; } = string.Empty;
  /// <summary>
  /// Gets or sets the extension of the phone.
  /// </summary>
  public string? Extension { get; set; }
  /// <summary>
  /// Gets or sets the E.164 formatted text of the phone.
  /// <br />See <see href="https://en.wikipedia.org/wiki/E.164"/> for more detail.
  /// </summary>
  public string E164Formatted { get; set; } = string.Empty;
}
