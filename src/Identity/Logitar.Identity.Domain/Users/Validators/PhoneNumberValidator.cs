using FluentValidation;

namespace Logitar.Identity.Domain.Users.Validators;

public class PhoneNumberValidator : AbstractValidator<IPhoneNumber>
{
  public PhoneNumberValidator()
  {
    When(x => x.CountryCode != null, () => RuleFor(x => x.CountryCode).NotEmpty()
      .MaximumLength(10));

    RuleFor(x => x.Number).NotEmpty()
      .MaximumLength(20);

    When(x => x.Extension != null, () => RuleFor(x => x.Extension).NotEmpty()
      .MaximumLength(10));

    RuleFor(x => x).Must(phone => phone.IsValid())
      .WithErrorCode("PhoneNumberValidator")
      .WithMessage("'{PropertyName}' must be a valid phone number.")
      .WithName(x => nameof(x.Number));
  }
}
