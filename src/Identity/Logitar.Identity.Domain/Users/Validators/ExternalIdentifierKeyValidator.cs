using FluentValidation;
using Logitar.Identity.Domain.Validators;

namespace Logitar.Identity.Domain.Users.Validators;

public class ExternalIdentifierKeyValidator : AbstractValidator<string>
{
  public ExternalIdentifierKeyValidator(string? propertyName = null)
  {
    IRuleBuilderOptions<string, string?> options = RuleFor(x => x).NotEmpty()
      .MaximumLength(byte.MaxValue)
      .Identifier();

    if (propertyName != null)
    {
      options = options.WithName(propertyName);
    }
  }
}
