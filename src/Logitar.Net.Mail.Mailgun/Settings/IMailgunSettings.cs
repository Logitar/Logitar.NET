namespace Logitar.Net.Mail.Mailgun.Settings;

/// <summary>
/// Defines the settings of the Mailgun API.
/// </summary>
public interface IMailgunSettings
{
  /// <summary>
  /// Gets the username used to authorize the Mailgun API calls.
  /// </summary>
  string Username { get; }
  /// <summary>
  /// Gets the password used to authorize the Mailgun API calls, typically your API key.
  /// </summary>
  string? Password { get; }
  /// <summary>
  /// Gets the API key used to authorize the Mailgun API calls.
  /// </summary>
  string? ApiKey { get; }

  /// <summary>
  /// Gets the base Uniform Resource Locator (URL) of the Mailgun API.
  /// </summary>
  string BaseUrl { get; }
  /// <summary>
  /// Gets the base Uniform Resource Identifier (URI) of the Mailgun API.
  /// </summary>
  Uri BaseUri { get; }

  /// <summary>
  /// Gets the name of the domain to send messages from.
  /// </summary>
  string? DomainName { get; }

  /// <summary>
  /// Gets the send mail end point information.
  /// </summary>
  EndPointSettings SendMail { get; }
}
