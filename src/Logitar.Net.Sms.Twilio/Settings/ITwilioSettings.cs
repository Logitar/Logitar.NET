namespace Logitar.Net.Sms.Twilio.Settings;

/// <summary>
/// Defines the settings of the Twilio API.
/// </summary>
public interface ITwilioSettings
{
  /// <summary>
  /// Gets the Twilio account Security IDentifier.
  /// </summary>
  string? AccountSid { get; }
  /// <summary>
  /// Gets the Twilio account authentication token.
  /// </summary>
  string? AuthenticationToken { get; }

  /// <summary>
  /// Gets the base Uniform Resource Locator (URL) of the Twilio API.
  /// </summary>
  string BaseUrl { get; }
  /// <summary>
  /// Gets the base Uniform Resource Identifier (URI) of the Twilio API.
  /// </summary>
  Uri BaseUri { get; }

  /// <summary>
  /// Gets the create message end point information.
  /// </summary>
  EndPointSettings CreateMessage { get; }
}
