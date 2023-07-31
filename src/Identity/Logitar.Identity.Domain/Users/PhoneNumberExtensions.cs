using PhoneNumbers;

namespace Logitar.Identity.Domain.Users;

public static class PhoneNumberExtensions
{
  public const string DefaultRegion = "US";

  public static string FormatToE164(this IPhoneNumber phoneNumber)
  {
    PhoneNumbers.PhoneNumber phone = phoneNumber.Parse();

    return PhoneNumberUtil.GetInstance().Format(phone, PhoneNumberFormat.E164);
  }

  public static bool IsValid(this IPhoneNumber phoneNumber)
  {
    try
    {
      _ = phoneNumber.Parse();

      return true;
    }
    catch (Exception)
    {
      return false;
    }
  }

  private static PhoneNumbers.PhoneNumber Parse(this IPhoneNumber phoneNumber)
  {
    StringBuilder phone = new();
    phone.Append(phoneNumber.Number.Trim());
    if (!string.IsNullOrWhiteSpace(phoneNumber.Extension))
    {
      phone.Append(" x").Append(phoneNumber.Extension.Trim());
    }

    return PhoneNumberUtil.GetInstance().Parse(phone.ToString(), phoneNumber.CountryCode ?? DefaultRegion);
  }
}
