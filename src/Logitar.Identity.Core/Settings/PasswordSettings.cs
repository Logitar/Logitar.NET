namespace Logitar.Identity.Core.Settings;

/// <summary>
/// The settings used to validate passwords.
/// </summary>
public record PasswordSettings : IPasswordSettings
{
  /// <summary>
  /// Gets or sets the required number of characters in a password.
  /// </summary>
  public int RequiredLength { get; set; }
  /// <summary>
  /// Gets or sets the required number of unique characters in a password.
  /// </summary>
  public int RequiredUniqueChars { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not passwords require a non-alphanumeric character.
  /// </summary>
  public bool RequireNonAlphanumeric { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not passwords require a lowercase letter.
  /// </summary>
  public bool RequireLowercase { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not passwords require an uppercase letter.
  /// </summary>
  public bool RequireUppercase { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not passwords require a digit.
  /// </summary>
  public bool RequireDigit { get; set; }
}
