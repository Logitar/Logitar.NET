using FluentValidation;
using Logitar.Identity.Domain.Users;
using System.Collections.Immutable;

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

  public static IRuleBuilderOptions<T, DateTime> Future<T>(this IRuleBuilder<T, DateTime> ruleBuilder, DateTime? moment = null)
  {
    return ruleBuilder.Must(d => BeInTheFuture(d, moment))
      .WithErrorCode("FutureValidator")
      .WithMessage("'{PropertyName}' must be in the future.");
  }
  internal static bool BeInTheFuture(DateTime dateTime, DateTime? moment) => dateTime > (moment ?? DateTime.UtcNow);

  public static IRuleBuilderOptions<T, string?> Identifier<T>(this IRuleBuilder<T, string?> ruleBuilder)
  {
    return ruleBuilder.Must(BeAValidIdentifier)
      .WithErrorCode("IdentifierValidator")
      .WithMessage("'{PropertyName}' may not start with a digit, and it may only contain letters, digits and underscores (_).");
  }
  internal static bool BeAValidIdentifier(string? identifier) => identifier == null
    || (!string.IsNullOrEmpty(identifier) && !char.IsDigit(identifier.First())
      && identifier.All(c => char.IsLetterOrDigit(c) || c == '_'));

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

  public static IRuleBuilderOptions<T, string?> PostalCode<T>(this IRuleBuilder<T, string?> ruleBuilder, string expression)
  {
    return ruleBuilder.Matches(expression)
      .WithErrorCode("PostalCodeValidator")
      .WithMessage(x => $"'{{PropertyName}}' must match the following expression: {expression}");
  }

  public static IRuleBuilderOptions<T, string?> Region<T>(this IRuleBuilder<T, string?> ruleBuilder, IImmutableSet<string> regions)
  {
    return ruleBuilder.Must(region => region == null || regions.Contains(region))
      .WithErrorCode("RegionValidator")
      .WithMessage(x => $"'{{PropertyName}}' must be one of the following: {string.Join(", ", regions)}");
  }
}
