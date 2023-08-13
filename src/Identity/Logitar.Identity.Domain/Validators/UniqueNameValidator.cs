using FluentValidation;
using Logitar.Identity.Domain.Settings;

namespace Logitar.Identity.Domain.Validators;

public class UniqueNameValidator : AbstractValidator<string>
{
  public UniqueNameValidator(IUniqueNameSettings uniqueNameSettings, string? propertyName = null)
  {
    IRuleBuilderOptions<string, string> options = RuleFor(x => x).NotEmpty()
      .MaximumLength(byte.MaxValue)
      .AllowedCharacters(uniqueNameSettings.AllowedCharacters);

    if (propertyName != null)
    {
      options.WithName(propertyName);
    }
  }
}
