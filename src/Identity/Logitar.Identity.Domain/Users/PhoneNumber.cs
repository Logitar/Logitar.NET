using FluentValidation;
using Logitar.Identity.Domain.Users.Validators;

namespace Logitar.Identity.Domain.Users;

public record PhoneNumber : Contact, IPhoneNumber
{
  public PhoneNumber(string number, string? countryCode = null, string? extension = null,
    bool isVerified = false) : base(isVerified)
  {
    CountryCode = countryCode?.CleanTrim();
    Number = number.Trim();
    Extension = extension?.CleanTrim();
    new PhoneNumberValidator().ValidateAndThrow(this);
  }

  public string? CountryCode { get; }
  public string Number { get; }
  public string? Extension { get; }
}
