using FluentValidation;
using Logitar.Identity.Core.Tokens.Payloads;

namespace Logitar.Identity.Core.Tokens.Validators;

public class CreateTokenValidator : AbstractValidator<CreateTokenPayload>
{
  public CreateTokenValidator()
  {
    RuleFor(x => x.Lifetime).GreaterThan(0);

    RuleFor(x => x.Secret).NotEmpty()
      .MinimumLength(256 / 8)
      .MaximumLength(512 / 8);

    RuleForEach(x => x.Claims).SetValidator(new TokenClaimValidator());
  }
}
