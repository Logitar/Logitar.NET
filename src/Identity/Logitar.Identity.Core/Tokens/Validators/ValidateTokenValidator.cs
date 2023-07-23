using FluentValidation;
using Logitar.Identity.Core.Tokens.Payloads;

namespace Logitar.Identity.Core.Tokens.Validators;

public class ValidateTokenValidator : AbstractValidator<ValidateTokenPayload>
{
  public ValidateTokenValidator()
  {
    RuleFor(x => x.Token).NotEmpty();

    RuleFor(x => x.Secret).NotEmpty()
      .MinimumLength(256 / 8)
      .MaximumLength(512 / 8);
  }
}
