namespace Logitar.Net.Mail.SendGrid.Settings;

/// <summary>
/// Represents a resolver for the SendGrid API settings, allowing developers to customize how those settings are resolved.
/// </summary>
public interface ISendGridSettingsResolver
{
  /// <summary>
  /// Resolves the SendGrid API settings.
  /// </summary>
  /// <returns>The SendGrid API settings.</returns>
  ISendGridSettings Resolve();
}
