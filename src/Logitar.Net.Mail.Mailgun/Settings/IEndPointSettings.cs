namespace Logitar.Net.Mail.Mailgun.Settings;

/// <summary>
/// Defines the settings of a Mailgun API endpoint.
/// </summary>
public interface IEndPointSettings
{
  /// <summary>
  /// Gets string representation of the endpoint's HTTP method.
  /// </summary>
  string Method { get; }
  /// <summary>
  /// Gets the HTTP method of the endpoint.
  /// </summary>
  HttpMethod HttpMethod { get; }

  /// <summary>
  /// Gets a string representation of the endpoint's path.
  /// </summary>
  string Path { get; }
  /// <summary>
  /// Gets the path of the endpoint.
  /// </summary>
  Uri UriPath { get; }
}
