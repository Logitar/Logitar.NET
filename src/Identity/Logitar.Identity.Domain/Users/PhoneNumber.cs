namespace Logitar.Identity.Domain.Users;

public record PhoneNumber : ContactInformation, IPhoneNumber
{
  public PhoneNumber(string number, string? countryCode = null, string? extension = null,
    bool isVerified = false) : base(isVerified)
  {
    CountryCode = countryCode?.CleanTrim();
    Number = number.Trim();
    Extension = extension?.CleanTrim();
  }

  public string? CountryCode { get; }
  public string Number { get; }
  public string? Extension { get; }

  public string FormatToE164() => PhoneNumberExtensions.FormatToE164(this);
}
