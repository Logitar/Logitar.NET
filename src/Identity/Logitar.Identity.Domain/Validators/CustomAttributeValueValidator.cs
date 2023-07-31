using FluentValidation;

namespace Logitar.Identity.Domain.Validators;

public class CustomAttributeValueValidator : AbstractValidator<string>
{
  public CustomAttributeValueValidator(string? propertyName = null)
  {
    IRuleBuilderOptions<string, string> options = RuleFor(x => x).NotEmpty();

    if (propertyName != null)
    {
      options = options.WithName(propertyName);
    }
  }
}
