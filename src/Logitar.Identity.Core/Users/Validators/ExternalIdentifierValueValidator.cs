using FluentValidation;

namespace Logitar.Identity.Core.Users.Validators;

/// <summary>
/// The validator used to validate external identifier values.
/// </summary>
public class ExternalIdentifierValueValidator : AbstractValidator<string>
{
  /// <summary>
  /// Initialize a new instance of the <see cref="ExternalIdentifierValueValidator"/> class.
  /// </summary>
  /// <param name="propertyName">The name of the validated property.</param>
  public ExternalIdentifierValueValidator(string? propertyName = null)
  {
    IRuleBuilderOptions<string, string?> options = RuleFor(x => x).NotEmpty()
      .MaximumLength(byte.MaxValue);

    if (propertyName != null)
    {
      options = options.WithName(propertyName);
    }
  }
}
