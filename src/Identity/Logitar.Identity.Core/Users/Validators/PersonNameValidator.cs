using FluentValidation;

namespace Logitar.Identity.Core.Users.Validators;
public class PersonNameValidator : AbstractValidator<string>
{
  public PersonNameValidator(string? propertyName = null)
  {
    IRuleBuilderOptions<string, string> options = RuleFor(x => x).NotEmpty()
      .MaximumLength(byte.MaxValue);

    if (propertyName != null)
    {
      options = options.WithName(propertyName);
    }
  }
}
