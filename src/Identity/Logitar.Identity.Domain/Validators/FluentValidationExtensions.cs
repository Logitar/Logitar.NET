using FluentValidation;

namespace Logitar.Identity.Domain.Validators;

public static class FluentValidationExtensions
{
  public const int LOCALE_CUSTOM_UNSPECIFIED = 0x1000;

  public static IRuleBuilderOptions<T, string?> AllowedCharacters<T>(this IRuleBuilder<T, string?> ruleBuilder, string? allowedCharacters)
  {
    return ruleBuilder.Must(s => ContainOnlyAllowedCharacters(s, allowedCharacters))
      .WithErrorCode("AllowedCharactersValidator")
      .WithMessage($"'{{PropertyName}}' may only contain the following characters: {allowedCharacters}");
  }
  internal static bool ContainOnlyAllowedCharacters(string? s, string? allowedCharacters)
    => s == null || allowedCharacters == null || s.All(allowedCharacters.Contains);

  public static IRuleBuilderOptions<T, CultureInfo?> Locale<T>(this IRuleBuilder<T, CultureInfo?> ruleBuilder)
  {
    return ruleBuilder.Must(BeAValidLocale)
      .WithErrorCode("LocaleValidator")
      .WithMessage("'{PropertyName}' may not be the invariant culture, nor a user-defined culture.");
  }
  internal static bool BeAValidLocale(CultureInfo? locale)
    => locale == null || (!string.IsNullOrEmpty(locale.Name) && locale.LCID != LOCALE_CUSTOM_UNSPECIFIED);
}
