using Logitar.Net.Http;

namespace Logitar.Net.Mail.Mailgun.Settings;

/// <summary>
/// Defines extension methods for the Mailgun API settings.
/// </summary>
public static class MailgunSettingsExtensions
{
  /// <summary>
  /// Returns an instance of HTTP API settings from the specified Mailgun API settings.
  /// </summary>
  /// <param name="settings">The Mailgun API settings.</param>
  /// <returns>The HTTP API settings.</returns>
  public static IHttpApiSettings ToHttpApiSettings(this IMailgunSettings settings)
  {
    HttpApiSettings apiSettings = new()
    {
      BaseUri = settings.BaseUri
    };

    if (!string.IsNullOrWhiteSpace(settings.Password))
    {
      apiSettings.Authorization = HttpAuthorization.Basic(settings.Username.Trim(), settings.Password.Trim());
    }

    return apiSettings;
  }
}
