namespace Logitar.Identity.Core.Users.Contact;

/// <summary>
/// TODO(fpion): document
/// </summary>
public record ReadOnlyPhone : ReadOnlyContact, IPhoneNumber
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ReadOnlyPhone"/> class.
  /// </summary>
  /// <param name="number">The number of the phone.</param>
  /// <param name="countryCode">The country code of the phone.</param>
  /// <param name="extension">The extension of the phone.</param>
  /// <param name="isVerified">A value indicating whether or not the phone is verified.</param>
  public ReadOnlyPhone(string number, string? countryCode = null, string? extension = null,
    bool isVerified = false) : base(isVerified)
  {
    CountryCode = countryCode?.CleanTrim();
    Number = number.Trim();
    Extension = extension?.CleanTrim();
    //E164Formatted = this.FormatToE164(); // TODO(fpion): implement
  }

  /// <summary>
  /// Gets the two-letter country code of the phone.
  /// <br />See <see href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2"/> for more detail.
  /// </summary>
  public string? CountryCode { get; }
  /// <summary>
  /// Gets the number of the phone.
  /// </summary>
  public string Number { get; }
  /// <summary>
  /// Gets the extension of the phone.
  /// </summary>
  public string? Extension { get; }
  /// <summary>
  /// Gets the E.164-formatted string representation of the phone.
  /// </summary>
  //public string E164Formatted { get; } // TODO(fpion): implement
}
