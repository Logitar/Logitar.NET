namespace Logitar.Identity.Core.Users.Contact;

/// <summary>
/// TODO(fpion): document
/// </summary>
public interface IPhoneNumber
{
  /// <summary>
  /// Gets the two-letter country code of the phone.
  /// <br />See <see href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2"/> for more detail.
  /// </summary>
  string? CountryCode { get; }
  /// <summary>
  /// Gets the number of the phone.
  /// </summary>
  string Number { get; }
  /// <summary>
  /// Gets the extension of the phone.
  /// </summary>
  string? Extension { get; }
}
