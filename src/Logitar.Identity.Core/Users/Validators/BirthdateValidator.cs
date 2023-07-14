using FluentValidation;

namespace Logitar.Identity.Core.Users.Validators;

/// <summary>
/// The validator used to validate birthdates.
/// </summary>
public class BirthdateValidator : AbstractValidator<DateTime>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="BirthdateValidator"/> class.
  /// </summary>
  /// <param name="propertyName">The name of the validated property.</param>
  public BirthdateValidator(string? propertyName = null)
  {
    IRuleBuilderOptions<DateTime, DateTime> options = RuleFor(x => x).Past();

    if (propertyName != null)
    {
      options = options.WithName(propertyName);
    }
  }
}
