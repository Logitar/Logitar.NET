namespace Logitar.Net.Mail.SendGrid.Settings;

/// <summary>
/// Defines the settings of the SendGrid API.
/// </summary>
public interface ISendGridSettings
{
  /// <summary>
  /// Gets the API key to authorize the SendGrid API calls.
  /// </summary>
  string? ApiKey { get; }

  /// <summary>
  /// Gets the base Uniform Resource Locator (URL) of the SendGrid API.
  /// </summary>
  string BaseUrl { get; }
  /// <summary>
  /// Gets the base Uniform Resource Identifier (URI) of the SendGrid API.
  /// </summary>
  Uri BaseUri { get; }

  /// <summary>
  /// Gets the send mail end point information.
  /// </summary>
  EndPointSettings SendMail { get; }
}
