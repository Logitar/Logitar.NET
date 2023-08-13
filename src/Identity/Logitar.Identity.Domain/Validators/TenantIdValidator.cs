using FluentValidation;

namespace Logitar.Identity.Domain.Validators;

public class TenantIdValidator : AbstractValidator<string>
{
  public TenantIdValidator(string? propertyName = null)
  {
    IRuleBuilderOptions<string, string> options = RuleFor(x => x).NotEmpty()
      .MaximumLength(byte.MaxValue)
      .AllowedCharacters(FluentValidationExtensions.UriSafeCharacters);

    if (propertyName != null)
    {
      options.WithName(propertyName);
    }
  }
}
