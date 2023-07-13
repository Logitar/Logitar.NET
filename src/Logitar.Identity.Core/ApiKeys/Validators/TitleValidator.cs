using FluentValidation;

namespace Logitar.Identity.Core.ApiKeys.Validators;

/// <summary>
/// The validator used to validate display names.
/// </summary>
public class TitleValidator : AbstractValidator<string>
{
  /// <summary>
  /// Initialize a new instance of the <see cref="TitleValidator"/> class.
  /// </summary>
  /// <param name="propertyName">The name of the validated property.</param>
  public TitleValidator(string? propertyName = null)
  {
    IRuleBuilderOptions<string, string?> options = RuleFor(x => x).NotEmpty()
      .MaximumLength(byte.MaxValue);

    if (propertyName != null)
    {
      options = options.WithName(propertyName);
    }
  }
}
