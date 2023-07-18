namespace Logitar.Identity.Core.Settings;

/// <summary>
/// The settings used to validate users.
/// </summary>
public record UserSettings : IUserSettings
{
  /// <summary>
  /// Gets or sets a value indicating whether or not users required a verified contact to sign-in to their account.
  /// </summary>
  public bool RequireConfirmedAccount { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not user email addresses are unique.
  /// </summary>
  public bool RequireUniqueEmail { get; set; }

  /// <summary>
  /// Gets or sets the settings used to validate user unique names.
  /// </summary>
  public IUniqueNameSettings UniqueNameSettings { get; set; } = new UniqueNameSettings();
  /// <summary>
  /// Gets or sets the settings used to validate user passwords.
  /// </summary>
  public IPasswordSettings PasswordSettings { get; set; } = new PasswordSettings();
}
