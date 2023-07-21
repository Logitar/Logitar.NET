using FluentValidation;
using Logitar.Identity.Core.Settings;

namespace Logitar.Identity.Core.Validators;

public class UniqueNameValidator : AbstractValidator<string>
{
  public UniqueNameValidator(IUniqueNameSettings uniqueNameSettings, string? propertyName = null)
  {
    var options = RuleFor(x => x).NotEmpty()
      .MaximumLength(byte.MaxValue)
      .AllowedCharacters(uniqueNameSettings.AllowedCharacters);

    if (propertyName != null)
    {
      options = options.WithName(propertyName);
    }
  }
}
