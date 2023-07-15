using FluentValidation;
using Logitar.Identity.Core.Users.Contact;

namespace Logitar.Identity.Core;

/// <summary>
/// Provides extension methods for the FluentValidation namespace.
/// </summary>
public static class FluentValidationExtensions
{
  /// <summary>
  /// Defines an only allowed characters validator on the specified rule builder.
  /// <br />Validation will fail if the input string contains characters that are not allowed.
  /// <br />Validation will succeed if the input string only contains characters that are allowed.
  /// </summary>
  /// <typeparam name="T">The type of the validated instance.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <param name="allowedCharacters">A string containing the list of allowed characters.</param>
  /// <returns>The rule builder.</returns>
  public static IRuleBuilderOptions<T, string?> AllowedCharacters<T>(this IRuleBuilder<T, string?> ruleBuilder, string? allowedCharacters)
  {
    return ruleBuilder.Must(s => ContainOnlyAllowedCharacters(s, allowedCharacters))
      .WithErrorCode(GetErrorCode(nameof(AllowedCharacters)))
      .WithMessage($"'{{PropertyName}}' may only contain the following characters: {allowedCharacters}");
  }
  /// <summary>
  /// Returns a value indicating whether or not the specified string only contains allowed characters.
  /// </summary>
  /// <param name="s">The input string to validate.</param>
  /// <param name="allowedCharacters">A string containing the list of allowed characters.</param>
  /// <returns>The validation result.</returns>
  internal static bool ContainOnlyAllowedCharacters(string? s, string? allowedCharacters)
  {
    return s == null || allowedCharacters == null || s.All(allowedCharacters.Contains);
  }

  /// <summary>
  /// Defines a future validator on the specified rule builder.
  /// <br />Validation will fail if the input date and time are prior to the validation moment.
  /// <br />Validation will succeed if the input date and time are future to the validation moment.
  /// </summary>
  /// <typeparam name="T">The type of the validated instance.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <param name="moment">The date and time to validate against. Defaults to now.</param>
  /// <returns>The rule builder.</returns>
  public static IRuleBuilderOptions<T, DateTime> Future<T>(this IRuleBuilder<T, DateTime> ruleBuilder, DateTime? moment = null)
  {
    return ruleBuilder.Must(d => BeInTheFuture(d, moment))
      .WithErrorCode(GetErrorCode(nameof(Future)))
      .WithMessage("'{PropertyName}' must be in the future.");
  }
  /// <summary>
  /// Returns a value indicating whether or not the specified date and time are in the future.
  /// </summary>
  /// <param name="value">The date and time to validate.</param>
  /// <param name="moment">The date and time to validate against. Defaults to now.</param>
  /// <returns>The validation result.</returns>
  internal static bool BeInTheFuture(DateTime value, DateTime? moment = null) => value > (moment ?? DateTime.Now);

  /// <summary>
  /// Defines an identifier validator on the specified rule builder.
  /// <br />Validation will fail if the input string starts with a digit, or contains a character that is not a letter, nor a digit, nor an underscore(_).
  /// <br />Validation will succeed if the input sting only contains letters, digits, or underscores (_), and do not start with a digit.
  /// </summary>
  /// <typeparam name="T">The type of the validated instance.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <returns>The rule builder.</returns>
  public static IRuleBuilderOptions<T, string?> Identifier<T>(this IRuleBuilder<T, string?> ruleBuilder)
  {
    return ruleBuilder.Must(BeAValidIdentifier)
      .WithErrorCode(GetErrorCode(nameof(Identifier)))
      .WithMessage("'{PropertyName}' may only contains letters, digits, and underscores (_), and may not start with a digit.");
  }
  /// <summary>
  /// Returns a value indicating whether or not the specified string is a valid identifier.
  /// <br />Valid identifiers may only contain letters, digits, and underscores (_), and may not start with a digit.
  /// </summary>
  /// <param name="s">The input string to validate.</param>
  /// <returns>The validation result.</returns>
  internal static bool BeAValidIdentifier(string? s) => (s == null)
    || (!string.IsNullOrWhiteSpace(s) && !char.IsDigit(s.First()) && s.All(c => char.IsLetterOrDigit(c) || c == '_'));

  /// <summary>
  /// Defines a locale validator on the specified rule builder.
  /// <br />Validation will fail if the input culture is the invariant culture, or if it is an user-defined culture.
  /// <br />Validation will succeed if the input culture is not the invariant culture nor an user-defined culture.
  /// </summary>
  /// <typeparam name="T">The type of the validated instance.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <returns>The rule builder.</returns>
  public static IRuleBuilderOptions<T, CultureInfo?> Locale<T>(this IRuleBuilder<T, CultureInfo?> ruleBuilder)
  {
    return ruleBuilder.Must(BeAValidLocale)
      .WithErrorCode(GetErrorCode(nameof(Locale)))
      .WithMessage("'{PropertyName}' may not be the invariant culture, and it may not be an user-defined culture.");
  }
  /// <summary>
  /// Returns a value indicating whether or not the specified culture is not the invariant culture and has a LCID different from 4096.
  /// <br />See <see href="https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo.lcid?view=net-7.0#remarks"/> for more detail.
  /// </summary>
  /// <param name="culture"></param>
  /// <returns></returns>
  internal static bool BeAValidLocale(CultureInfo? culture) => (culture == null)
    || (!string.IsNullOrEmpty(culture.Name) && culture.LCID != 4096);

  /// <summary>
  /// Defines a null or not empty validator on the specified rule builder.
  /// <br />Validation will fail if the input string is empty or only white space.
  /// <br />Validation will succeed if the input string is null, or is not empty nor only white space.
  /// </summary>
  /// <typeparam name="T">The type of the validated instance.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <returns>The rule builder.</returns>
  public static IRuleBuilderOptions<T, string?> NullOrNotEmpty<T>(this IRuleBuilder<T, string?> ruleBuilder)
  {
    return ruleBuilder.Must(BeNullOrNotEmpty)
      .WithErrorCode(GetErrorCode(nameof(NullOrNotEmpty)))
      .WithMessage("'{PropertyName}' must be a null string, or it must be not empty nor only white space.");
  }
  /// <summary>
  /// Returns a value indicating whether or not the specified string is null, or is not empty nor only white space.
  /// </summary>
  /// <param name="s">The input string to validate.</param>
  /// <returns>The validation result.</returns>
  internal static bool BeNullOrNotEmpty(string? s) => s == null || !string.IsNullOrWhiteSpace(s);

  /// <summary>
  /// Defines a past validator on the specified rule builder.
  /// <br />Validation will fail if the input date and time are future to the validation moment.
  /// <br />Validation will succeed if the input date and time are prior to the validation moment.
  /// </summary>
  /// <typeparam name="T">The type of the validated instance.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <param name="moment">The date and time to validate against. Defaults to now.</param>
  /// <returns>The rule builder.</returns>
  public static IRuleBuilderOptions<T, DateTime> Past<T>(this IRuleBuilder<T, DateTime> ruleBuilder, DateTime? moment = null)
  {
    return ruleBuilder.Must(d => BeInThePast(d, moment))
      .WithErrorCode(GetErrorCode(nameof(Past)))
      .WithMessage("'{PropertyName}' must be in the past.");
  }
  /// <summary>
  /// Returns a value indicating whether or not the specified date and time are in the past.
  /// </summary>
  /// <param name="value">The date and time to validate.</param>
  /// <param name="moment">The date and time to validate against. Defaults to now.</param>
  /// <returns>The validation result.</returns>
  internal static bool BeInThePast(DateTime value, DateTime? moment = null) => value < (moment ?? DateTime.Now);

  /// <summary>
  /// Defines a phone number validator on the specified rule builder.
  /// <br />Validation will fail if the input phone number is not valid according to the libphonenumber library.
  /// <br />Validation will succeed if the input phone number is valid according to the libphonenumber library.
  /// </summary>
  /// <typeparam name="T">The type of the validated instance.</typeparam>
  /// <param name="ruleBuilder">The rule builder.</param>
  /// <returns>The rule builder.</returns>
  public static IRuleBuilderOptions<T, IPhoneNumber?> PhoneNumber<T>(this IRuleBuilder<T, IPhoneNumber>? ruleBuilder)
  {
    return ruleBuilder.Must(BeAValidPhoneNumber)
      .WithErrorCode(GetErrorCode(nameof(PhoneNumber)))
      .WithMessage("'{PropertyName}' must be a valid phone number.");
  }
  /// <summary>
  /// Returns a value indicating whether or not the specified phone is valid according to the libphonenumber library.
  /// </summary>
  /// <param name="phone">The phone number to validate.</param>
  /// <returns>The validation result.</returns>
  internal static bool BeAValidPhoneNumber(IPhoneNumber? phone) => phone?.IsValid() != false;

  /// <summary>
  /// Constructs an error code from the specified method name.
  /// </summary>
  /// <param name="methodName">The name of the method.</param>
  /// <returns>The constructed error code.</returns>
  private static string GetErrorCode(string methodName) => string.Concat(methodName, "Validator");
}
