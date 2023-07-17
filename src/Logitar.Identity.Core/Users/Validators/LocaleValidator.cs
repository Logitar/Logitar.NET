using FluentValidation;

namespace Logitar.Identity.Core.Users.Validators;

/// <summary>
/// The validator used to validate locales.
/// </summary>
public class LocaleValidator : AbstractValidator<CultureInfo>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="LocaleValidator"/> class.
  /// </summary>
  /// <param name="propertyName">The name of the validated property.</param>
  public LocaleValidator(string? propertyName = null)
  {
    IRuleBuilderOptions<CultureInfo, CultureInfo?> options = RuleFor(x => x).Locale();

    if (propertyName != null)
    {
      options = options.WithName(propertyName);
    }
  }
}
