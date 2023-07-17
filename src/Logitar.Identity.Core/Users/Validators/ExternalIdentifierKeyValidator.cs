using FluentValidation;

namespace Logitar.Identity.Core.Users.Validators;

/// <summary>
/// The validator used to validate external identifier keys.
/// </summary>
public class ExternalIdentifierKeyValidator : AbstractValidator<string>
{
  /// <summary>
  /// Initialize a new instance of the <see cref="ExternalIdentifierKeyValidator"/> class.
  /// </summary>
  /// <param name="propertyName">The name of the validated property.</param>
  public ExternalIdentifierKeyValidator(string? propertyName = null)
  {
    IRuleBuilderOptions<string, string?> options = RuleFor(x => x).NotEmpty()
      .MaximumLength(byte.MaxValue)
      .Identifier();

    if (propertyName != null)
    {
      options = options.WithName(propertyName);
    }
  }
}
