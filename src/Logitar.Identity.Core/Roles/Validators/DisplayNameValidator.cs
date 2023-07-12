using FluentValidation;

namespace Logitar.Identity.Core.Roles.Validators;

/// <summary>
/// The validator used to validate display names.
/// </summary>
public class DisplayNameValidator : AbstractValidator<string>
{
  /// <summary>
  /// Initialize a new instance of the <see cref="DisplayNameValidator"/> class.
  /// </summary>
  /// <param name="propertyName">The name of the validated property.</param>
  public DisplayNameValidator(string? propertyName = null)
  {
    IRuleBuilderOptions<string, string?> options = RuleFor(x => x).NullOrNotEmpty()
      .MaximumLength(byte.MaxValue);

    if (propertyName != null)
    {
      options = options.WithName(propertyName);
    }
  }
}
