using Logitar.Net.Http;

namespace Logitar.Net.Sms.Twilio.Settings;

/// <summary>
/// Defines extension methods for the Twilio API settings.
/// </summary>
public static class TwilioSettingsExtensions
{
  /// <summary>
  /// Returns an instance of HTTP API settings from the specified Twilio API settings.
  /// </summary>
  /// <param name="settings">The Twilio API settings.</param>
  /// <returns>The HTTP API settings.</returns>
  public static IHttpApiSettings ToHttpApiSettings(this ITwilioSettings settings)
  {
    HttpApiSettings apiSettings = new()
    {
      BaseUri = settings.BaseUri
    };

    if (!string.IsNullOrWhiteSpace(settings.AccountSid) && !string.IsNullOrWhiteSpace(settings.AuthenticationToken))
    {
      apiSettings.Authorization = HttpAuthorization.Basic(settings.AccountSid.Trim(), settings.AuthenticationToken.Trim());
    }

    return apiSettings;
  }
}
