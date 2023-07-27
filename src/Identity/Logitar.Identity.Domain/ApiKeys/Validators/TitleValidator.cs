using FluentValidation;

namespace Logitar.Identity.Domain.ApiKeys.Validators;

public class TitleValidator : AbstractValidator<string>
{
  public TitleValidator(string? propertyName = null)
  {
    IRuleBuilderOptions<string, string> options = RuleFor(x => x).NotEmpty()
      .MaximumLength(byte.MaxValue);

    if (propertyName != null)
    {
      options = options.WithName(propertyName);
    }
  }
}
