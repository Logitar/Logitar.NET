using FluentValidation;

namespace Logitar.Identity.Core;

/// <summary>
/// Provides extension methods for the FluentValidation namespace.
/// </summary>
public static class FluentValidationExtensions
{
  /// <summary>
  /// Defines an only allowed characters validator to the specified rule builder.
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
  private static bool ContainOnlyAllowedCharacters(string? s, string? allowedCharacters)
  {
    return s == null || allowedCharacters == null || s.All(allowedCharacters.Contains);
  }

  /// <summary>
  /// Defines an identifier validator to the specified rule builder.
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
  private static bool BeAValidIdentifier(string? s) => (s == null)
    || (!string.IsNullOrWhiteSpace(s) && !char.IsDigit(s.First()) && s.All(c => char.IsLetterOrDigit(c) || c == '_'));

  /// <summary>
  /// Defines a null or not empty validator to the specified rule builder.
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
  private static bool BeNullOrNotEmpty(string? s) => s == null || !string.IsNullOrWhiteSpace(s);

  /// <summary>
  /// Constructs an error code from the specified method name.
  /// </summary>
  /// <param name="methodName">The name of the method.</param>
  /// <returns>The constructed error code.</returns>
  private static string GetErrorCode(string methodName) => string.Concat(methodName, "Validator");
}
