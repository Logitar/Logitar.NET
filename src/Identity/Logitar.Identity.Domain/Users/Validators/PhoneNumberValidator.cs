using FluentValidation;
using Logitar.Identity.Domain.Validators;

namespace Logitar.Identity.Domain.Users.Validators;

public class PhoneNumberValidator : AbstractValidator<IPhoneNumber>
{
  public PhoneNumberValidator()
  {
    When(x => x.CountryCode != null,
      () => RuleFor(x => x.CountryCode).NotEmpty()
        .Length(2));

    RuleFor(x => x.Number).NotEmpty()
      .MaximumLength(16);

    When(x => x.Extension != null,
      () => RuleFor(x => x.Extension).NotEmpty()
        .MaximumLength(10));

    RuleFor(x => x).PhoneNumber()
      .WithName(x => x.Number);
  }
}
