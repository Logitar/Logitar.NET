using FluentValidation;
using Logitar.Identity.Core.Tokens.Payloads;

namespace Logitar.Identity.Core.Tokens.Validators;

public class TokenClaimValidator : AbstractValidator<TokenClaim>
{
  public TokenClaimValidator()
  {
    RuleFor(x => x.Type).NotEmpty();

    RuleFor(x => x.Value).NotEmpty();
  }
}
