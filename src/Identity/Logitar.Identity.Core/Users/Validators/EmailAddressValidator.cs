using FluentValidation;

namespace Logitar.Identity.Core.Users.Validators;

public class EmailAddressValidator : AbstractValidator<IEmailAddress>
{
  public EmailAddressValidator()
  {
    RuleFor(x => x.Address).NotEmpty()
      .MaximumLength(byte.MaxValue)
      .EmailAddress();
  }
}
