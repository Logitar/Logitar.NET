using FluentValidation;

namespace Logitar.Identity.Core.Users.Validators;

/// <summary>
/// The validator used to validate person names.
/// </summary>
public class PersonNameValidator : AbstractValidator<string>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="PersonNameValidator"/> class.
  /// </summary>
  /// <param name="propertyName">The name of the validated property.</param>
  public PersonNameValidator(string? propertyName = null)
  {
    IRuleBuilderOptions<string, string> options = RuleFor(x => x).NullOrNotEmpty()
      .MaximumLength(byte.MaxValue);

    if (propertyName != null)
    {
      options = options.WithName(propertyName);
    }
  }
}
