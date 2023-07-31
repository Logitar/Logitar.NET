using FluentValidation;
using Logitar.Security.Cryptography;

namespace Logitar.Identity.Core.Passwords;

public class Pbkdf2Validator : AbstractValidator<Pbkdf2Settings>
{
  public Pbkdf2Validator()
  {
    RuleFor(x => x.Algorithm).IsInEnum();

    RuleFor(x => x.Iterations).GreaterThan(0);

    RuleFor(x => x.SaltLength).GreaterThan(0);

    RuleFor(x => x.HashLength).GreaterThan(0);
  }
}
