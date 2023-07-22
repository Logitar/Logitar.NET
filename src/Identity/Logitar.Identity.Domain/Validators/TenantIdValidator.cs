using FluentValidation;

namespace Logitar.Identity.Domain.Validators;

public class TenantIdValidator : AbstractValidator<string>
{
  public const string AllowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._~";

  public TenantIdValidator(string? propertyName = null)
  {
    IRuleBuilderOptions<string, string?> options = RuleFor(x => x).NotEmpty()
      .MaximumLength(byte.MaxValue)
      .AllowedCharacters(AllowedCharacters);

    if (propertyName != null)
    {
      options = options.WithName(propertyName);
    }
  }
}
