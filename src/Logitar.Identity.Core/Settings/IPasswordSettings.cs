namespace Logitar.Identity.Core.Settings;

/// <summary>
/// The settings used to validate passwords.
/// </summary>
public interface IPasswordSettings
{
  /// <summary>
  /// Gets the required number of characters in a password.
  /// </summary>
  int RequiredLength { get; }
  /// <summary>
  /// Gets the required number of unique characters in a password.
  /// </summary>
  int RequiredUniqueChars { get; }
  /// <summary>
  /// Gets a value indicating whether or not passwords require a non-alphanumeric character.
  /// </summary>
  bool RequireNonAlphanumeric { get; }
  /// <summary>
  /// Gets a value indicating whether or not passwords require a lowercase letter.
  /// </summary>
  bool RequireLowercase { get; }
  /// <summary>
  /// Gets a value indicating whether or not passwords require an uppercase letter.
  /// </summary>
  bool RequireUppercase { get; }
  /// <summary>
  /// Gets a value indicating whether or not passwords require a digit.
  /// </summary>
  bool RequireDigit { get; }
}
