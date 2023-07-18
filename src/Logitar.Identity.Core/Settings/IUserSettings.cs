namespace Logitar.Identity.Core.Settings;

/// <summary>
/// The settings used to validate users.
/// </summary>
public interface IUserSettings
{
  /// <summary>
  /// Gets a value indicating whether or not users required a verified contact to sign-in to their account.
  /// </summary>
  bool RequireConfirmedAccount { get; }
  /// <summary>
  /// Gets a value indicating whether or not user email addresses are unique.
  /// </summary>
  bool RequireUniqueEmail { get; }

  /// <summary>
  /// Gets the settings used to validate user unique names.
  /// </summary>
  IUniqueNameSettings UniqueNameSettings { get; }
  /// <summary>
  /// Gets the settings used to validate user passwords.
  /// </summary>
  IPasswordSettings PasswordSettings { get; }
}
