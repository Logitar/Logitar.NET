using FluentValidation;

namespace Logitar.Identity.Core.Validators;

/// <summary>
/// The validator used to validate custom attribute values.
/// </summary>
public class CustomAttributeValueValidator : AbstractValidator<string>
{
  /// <summary>
  /// Initialize a new instance of the <see cref="CustomAttributeValueValidator"/> class.
  /// </summary>
  /// <param name="propertyName">The name of the validated property.</param>
  public CustomAttributeValueValidator(string? propertyName = null)
  {
    IRuleBuilderOptions<string, string?> options = RuleFor(x => x).NotEmpty();

    if (propertyName != null)
    {
      options = options.WithName(propertyName);
    }
  }
}
