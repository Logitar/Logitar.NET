using PhoneNumbers;

namespace Logitar.Identity.Domain.Users;

public static class PhoneNumberExtensions
{
  private static readonly PhoneNumberUtil _util = PhoneNumberUtil.GetInstance();

  public static string DefaultRegion { get; set; } = "US";

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

  public static string FormatToE164(this IPhoneNumber phoneNumber)
  {
    PhoneNumbers.PhoneNumber phone = phoneNumber.Parse();

    return _util.Format(phone, PhoneNumberFormat.E164);
  }

  private static PhoneNumbers.PhoneNumber Parse(this IPhoneNumber phoneNumber)
  {
    string phone = string.IsNullOrEmpty(phoneNumber.Extension)
      ? phoneNumber.Number
      : $"{phoneNumber.Number} x{phoneNumber.Extension}";

    return _util.Parse(phone.ToString(), phoneNumber.CountryCode ?? DefaultRegion);
  }
}
