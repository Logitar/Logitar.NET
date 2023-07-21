using FluentValidation;

namespace Logitar.Identity.Core.Validators;

public class LocaleValidator : AbstractValidator<CultureInfo>
{
  public LocaleValidator(string? propertyName = null)
  {
    IRuleBuilderOptions<CultureInfo, CultureInfo?> options = RuleFor(x => x).Locale();

    if (propertyName != null)
    {
      options = options.WithName(propertyName);
    }
  }
}
