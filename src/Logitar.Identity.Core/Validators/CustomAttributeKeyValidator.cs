using FluentValidation;

namespace Logitar.Identity.Core.Validators;

/// <summary>
/// The validator used to validate custom attribute keys.
/// </summary>
public class CustomAttributeKeyValidator : AbstractValidator<string>
{
  /// <summary>
  /// Initialize a new instance of the <see cref="CustomAttributeKeyValidator"/> class.
  /// </summary>
  /// <param name="propertyName">The name of the validated property.</param>
  public CustomAttributeKeyValidator(string? propertyName = null)
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
