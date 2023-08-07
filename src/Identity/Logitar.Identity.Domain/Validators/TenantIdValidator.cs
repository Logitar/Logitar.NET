using FluentValidation;

namespace Logitar.Identity.Domain.Validators;

public class TenantIdValidator : AbstractValidator<string>
{
  private const string UriSafeCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._~";

  public TenantIdValidator(string? propertyName = null)
  {
    IRuleBuilderOptions<string, string> options = RuleFor(x => x).NotEmpty()
      .MaximumLength(byte.MaxValue)
      .AllowedCharacters(UriSafeCharacters);

    if (propertyName != null)
    {
      options.WithName(propertyName);
    }
  }
}
