using FluentValidation;

namespace Logitar.Identity.Core.ApiKeys.Validators;

/// <summary>
/// The validator used to validate expiration date and times.
/// </summary>
public class ExpiresOnValidator : AbstractValidator<DateTime>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ExpiresOnValidator"/> class.
  /// </summary>
  /// <param name="propertyName">The name of the validated property.</param>
  public ExpiresOnValidator(string? propertyName = null)
  {
    IRuleBuilderOptions<DateTime, DateTime> options = RuleFor(x => x).Future();

    if (propertyName != null)
    {
      options = options.WithName(propertyName);
    }
  }
}
