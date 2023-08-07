using FluentValidation;

namespace Logitar.Identity.Domain.Validators;

public static class FluentValidationExtensions
{
  private const int LOCALE_CUSTOM_UNSPECIFIED = 0x1000;

  public static IRuleBuilderOptions<T, string> AllowedCharacters<T>(this IRuleBuilder<T, string> ruleBuilder, string? allowedCharacters)
  {
    return ruleBuilder.Must(value => OnlyContainAllowedCharacters(value, allowedCharacters))
      .WithErrorCode(BuildErrorCode(nameof(AllowedCharacters)))
      .WithMessage($"'{{PropertyName}}' may only contain the following characters: {allowedCharacters}");
  }
  private static bool OnlyContainAllowedCharacters(string value, string? allowedCharacters)
    => allowedCharacters == null || value.All(allowedCharacters.Contains);

  public static IRuleBuilderOptions<T, DateTime> Future<T>(this IRuleBuilder<T, DateTime> ruleBuilder, DateTime? moment = null)
  {
    return ruleBuilder.Must(value => BeInTheFuture(value, moment))
      .WithErrorCode(BuildErrorCode(nameof(Past)))
      .WithMessage("'{PropertyName}' must be a date set in the future.");
  }
  private static bool BeInTheFuture(DateTime value, DateTime? moment = null) => value > (moment ?? DateTime.Now);

  public static IRuleBuilderOptions<T, string> Identifier<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.Must(BeAValidIdentifier)
      .WithErrorCode(BuildErrorCode(nameof(Identifier)))
      .WithMessage("'{PropertyName}' may not start with a digit, and it may only contain letters, digits and underscores (_).");
  }
  private static bool BeAValidIdentifier(string value) => !string.IsNullOrEmpty(value)
    && !char.IsDigit(value.First()) && value.All(c => char.IsLetterOrDigit(c) || c == '_');

  public static IRuleBuilderOptions<T, CultureInfo> Locale<T>(this IRuleBuilder<T, CultureInfo> ruleBuilder)
  {
    return ruleBuilder.Must(BeAValidLocale)
      .WithErrorCode(BuildErrorCode(nameof(Locale)))
      .WithMessage("'{PropertyName}' may not be the invariant culture, and it cannot be an user-defined culture.");
  }
  private static bool BeAValidLocale(CultureInfo culture)
    => !string.IsNullOrWhiteSpace(culture.Name) && culture.LCID != LOCALE_CUSTOM_UNSPECIFIED;

  public static IRuleBuilderOptions<T, DateTime> Past<T>(this IRuleBuilder<T, DateTime> ruleBuilder, DateTime? moment = null)
  {
    return ruleBuilder.Must(value => BeInThePast(value, moment))
      .WithErrorCode(BuildErrorCode(nameof(Past)))
      .WithMessage("'{PropertyName}' must be a date set in the past.");
  }
  private static bool BeInThePast(DateTime value, DateTime? moment = null) => value < (moment ?? DateTime.Now);

  private static string BuildErrorCode(string name) => $"{name}Validator";
}
