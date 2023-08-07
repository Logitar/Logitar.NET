using FluentValidation;

namespace Logitar.Identity.Domain.Validators;

public static class FluentValidationExtensions
{
  public static IRuleBuilderOptions<T, string> AllowedCharacters<T>(this IRuleBuilder<T, string> ruleBuilder, string? allowedCharacters)
  {
    return ruleBuilder.Must(value => OnlyContainAllowedCharacters(value, allowedCharacters))
      .WithErrorCode(BuildErrorCode(nameof(AllowedCharacters)))
      .WithMessage($"'{{PropertyName}}' may only contain the following characters: {allowedCharacters}");
  }
  private static bool OnlyContainAllowedCharacters(string value, string? allowedCharacters)
    => allowedCharacters == null || value.All(allowedCharacters.Contains);

  public static IRuleBuilderOptions<T, string> Identifier<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.Must(BeAValidIdentifier)
      .WithErrorCode(BuildErrorCode(nameof(Identifier)))
      .WithMessage("'{PropertyName}' may not start with a digit, and it may only contain letters, digits and underscores (_).");
  }
  private static bool BeAValidIdentifier(string value) => !string.IsNullOrEmpty(value)
    && !char.IsDigit(value.First()) && value.All(c => char.IsLetterOrDigit(c) || c == '_');

  private static string BuildErrorCode(string name) => $"{name}Validator";
}
