using FluentValidation;

namespace Logitar.Identity.Domain.Validators;

public class CustomAttributeKeyValidator : AbstractValidator<string>
{
  public CustomAttributeKeyValidator(string? propertyName = null)
  {
    IRuleBuilderOptions<string, string> options = RuleFor(x => x).NotEmpty()
      .MaximumLength(byte.MaxValue)
      .Identifier();

    if (propertyName != null)
    {
      options.WithName(propertyName);
    }
  }
}
