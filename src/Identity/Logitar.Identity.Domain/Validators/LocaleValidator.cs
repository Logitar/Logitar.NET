using FluentValidation;
using System.Globalization;

namespace Logitar.Identity.Domain.Validators;

public class LocaleValidator : AbstractValidator<CultureInfo>
{
  public LocaleValidator(string? propertyName = null)
  {
    IRuleBuilderOptions<CultureInfo, CultureInfo> options = RuleFor(x => x).NotNull()
      .Locale();

    if (propertyName != null)
    {
      options.WithName(propertyName);
    }
  }
}
