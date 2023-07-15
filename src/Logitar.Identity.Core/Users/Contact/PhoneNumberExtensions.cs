using PhoneNumbers;

namespace Logitar.Identity.Core.Users.Contact;

/// <summary>
/// Provides extension methods for instances of <see cref="IPhoneNumber"/>.
/// </summary>
public static class PhoneNumberExtensions
{
  /// <summary>
  /// The default phone country code.
  /// </summary>
  private const string DefaultRegion = "US";

  /// <summary>
  /// Formats the specified phone to E.164.
  /// </summary>
  /// <param name="phoneNumber">The phone number to format.</param>
  /// <returns>The formatted E.164 string.</returns>
  public static string FormatToE164(this IPhoneNumber phoneNumber)
  {
    PhoneNumber phone = phoneNumber.Parse();

    return PhoneNumberUtil.GetInstance().Format(phone, PhoneNumberFormat.E164);
  }

  /// <summary>
  /// Returns a value indicating whether or not the specified phone is valid.
  /// </summary>
  /// <param name="phoneNumber">The phone to validate.</param>
  /// <returns>The validation result.</returns>
  public static bool IsValid(this IPhoneNumber phoneNumber)
  {
    try
    {
      _ = phoneNumber.Parse();

      return true;
    }
    catch (NumberParseException)
    {
      return false;
    }
  }

  /// <summary>
  /// Parse the specified phone to an instance of a phone in the libphonenumber library.
  /// <br />See <see href="https://github.com/twcclegg/libphonenumber-csharp"/> for more detail.
  /// </summary>
  /// <param name="phoneNumber">The phone to parse.</param>
  /// <returns>The library phone instance.</returns>
  private static PhoneNumber Parse(this IPhoneNumber phoneNumber)
  {
    string phone = string.IsNullOrEmpty(phoneNumber.Extension)
      ? phoneNumber.Number
      : $"{phoneNumber.Number} x{phoneNumber.Extension}";

    return PhoneNumberUtil.GetInstance().Parse(phone.ToString(), phoneNumber.CountryCode ?? DefaultRegion);
  }
}
