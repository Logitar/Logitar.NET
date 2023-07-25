using FluentValidation;
using Logitar.Identity.Domain.Users;

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

  public static IRuleBuilderOptions<T, string> Country<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.Must(PostalAddressHelper.IsSupported)
      .WithErrorCode("CountryValidator")
      .WithMessage($"'{{PropertyName}}' must be one of the following: {string.Join(", ", PostalAddressHelper.SupportedCountries)}");
  }

  public static IRuleBuilderOptions<T, CultureInfo?> Locale<T>(this IRuleBuilder<T, CultureInfo?> ruleBuilder)
  {
    return ruleBuilder.Must(BeAValidLocale)
      .WithErrorCode("LocaleValidator")
      .WithMessage("'{PropertyName}' may not be the invariant culture, nor a user-defined culture.");
  }
  internal static bool BeAValidLocale(CultureInfo? locale)
    => locale == null || (!string.IsNullOrEmpty(locale.Name) && locale.LCID != LOCALE_CUSTOM_UNSPECIFIED);

  public static IRuleBuilderOptions<T, DateTime> Past<T>(this IRuleBuilder<T, DateTime> ruleBuilder, DateTime? moment = null)
  {
    return ruleBuilder.Must(d => BeInThePast(d, moment))
      .WithErrorCode("PastValidator")
      .WithMessage("'{PropertyName}' must be in the past.");
  }
  internal static bool BeInThePast(DateTime dateTime, DateTime? moment) => dateTime < (moment ?? DateTime.UtcNow);

  public static IRuleBuilderOptions<T, IPhoneNumber?> PhoneNumber<T>(this IRuleBuilder<T, IPhoneNumber> ruleBuilder)
  {
    return ruleBuilder.Must(phone => phone.IsValid())
      .WithErrorCode("PhoneNumberValidator")
      .WithMessage("'{PropertyName}' must be a valid phone number.");
  }
}
