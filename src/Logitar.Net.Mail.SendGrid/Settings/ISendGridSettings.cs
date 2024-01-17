namespace Logitar.Net.Mail.SendGrid.Settings;

/// <summary>
/// The settings required to send emails from the SendGrid API.
/// </summary>
public interface ISendGridSettings
{
  /// <summary>
  /// Gets the authentication/authorization settings of the SendGrid API.
  /// </summary>
  IAuthorizationSettings Authorization { get; }

  /// <summary>
  /// Gets the base Uniform Resource Locator (URL) of the SendGrid API.
  /// </summary>
  string BaseUrl { get; }
  /// <summary>
  /// Gets the base Uniform Resource Identifier (URI) of the SendGrid API.
  /// </summary>
  Uri BaseUri { get; }

  /// <summary>
  /// Gets the send mail endpoint of the SendGrid API.
  /// </summary>
  IEndPointSettings SendMail { get; }
}
