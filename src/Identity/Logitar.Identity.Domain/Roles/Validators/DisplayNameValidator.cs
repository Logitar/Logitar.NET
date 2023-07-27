using FluentValidation;

namespace Logitar.Identity.Domain.Roles.Validators;

public class DisplayNameValidator : AbstractValidator<string>
{
  public DisplayNameValidator(string? propertyName = null)
  {
    IRuleBuilderOptions<string, string> options = RuleFor(x => x).NotEmpty()
      .MaximumLength(byte.MaxValue);

    if (propertyName != null)
    {
      options = options.WithName(propertyName);
    }
  }
}
