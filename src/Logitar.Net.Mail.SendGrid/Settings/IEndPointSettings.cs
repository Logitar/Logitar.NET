namespace Logitar.Net.Mail.SendGrid.Settings;

/// <summary>
/// The settings of an endpoint.
/// </summary>
public interface IEndPointSettings
{
  /// <summary>
  /// Gets a string representation of the endpoint's HTTP method.
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
  Uri PathUri { get; }
}
