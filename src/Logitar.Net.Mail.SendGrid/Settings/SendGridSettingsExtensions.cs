using Logitar.Net.Http;

namespace Logitar.Net.Mail.SendGrid.Settings;

/// <summary>
/// Defines extension methods for the SendGrid API settings.
/// </summary>
public static class SendGridSettingsExtensions
{
  /// <summary>
  /// Returns an instance of HTTP API settings from the specified SendGrid API settings.
  /// </summary>
  /// <param name="settings">The SendGrid API settings.</param>
  /// <returns>The HTTP API settings.</returns>
  public static IHttpApiSettings ToHttpApiSettings(this ISendGridSettings settings)
  {
    HttpApiSettings apiSettings = new()
    {
      BaseUri = settings.BaseUri
    };

    if (!string.IsNullOrWhiteSpace(settings.ApiKey))
    {
      apiSettings.Authorization = HttpAuthorization.Bearer(settings.ApiKey.Trim());
    }

    return apiSettings;
  }
}
