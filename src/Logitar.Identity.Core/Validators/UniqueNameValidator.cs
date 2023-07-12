using FluentValidation;
using Logitar.Identity.Core.Settings;

namespace Logitar.Identity.Core.Validators;

/// <summary>
/// The validator used to validate unique names.
/// </summary>
public class UniqueNameValidator : AbstractValidator<string>
{
  /// <summary>
  /// Initialize a new instance of the <see cref="UniqueNameValidator"/> class.
  /// </summary>
  /// <param name="uniqueNameSettings">The settings used to validate the unique name.</param>
  /// <param name="propertyName">The name of the validated property.</param>
  public UniqueNameValidator(IUniqueNameSettings uniqueNameSettings, string? propertyName = null)
  {
    IRuleBuilderOptions<string, string?> options = RuleFor(x => x).NotEmpty()
      .MaximumLength(byte.MaxValue)
      .AllowedCharacters(uniqueNameSettings.AllowedCharacters);

    if (propertyName != null)
    {
      options = options.WithName(propertyName);
    }
  }
}
