namespace Logitar.Net.Mail.SendGrid.Settings;

/// <summary>
/// Represents an endpoint of the SendGrid API.
/// </summary>
public record EndPointSettings : IEndPointSettings
{
  /// <summary>
  /// Gets a string representation of the endpoint's HTTP method.
  /// </summary>
  public string Method { get; set; } = string.Empty;
  /// <summary>
  /// Gets the HTTP method of the endpoint.
  /// </summary>
  public HttpMethod HttpMethod => new(Method);

  /// <summary>
  /// Gets a string representation of the endpoint's path.
  /// </summary>
  public string Path { get; set; } = string.Empty;
  /// <summary>
  /// Gets the path of the endpoint.
  /// </summary>
  public Uri PathUri => new(Path, UriKind.Relative);
}
