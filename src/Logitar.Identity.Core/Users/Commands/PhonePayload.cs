namespace Logitar.Identity.Core.Users.Commands;

/// <summary>
/// The phone number input data.
/// </summary>
public record PhonePayload : ContactPayload
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
}
