namespace Logitar.Net.Sms.Twilio.Settings;

/// <summary>
/// Implements the settings of a Twilio API endpoint.
/// </summary>
public record EndPointSettings : IEndPointSettings
{
  /// <summary>
  /// Gets or sets string representation of the endpoint's HTTP method.
  /// </summary>
  public string Method { get; set; } = HttpMethod.Get.Method;
  /// <summary>
  /// Gets the HTTP method of the endpoint.
  /// </summary>
  public HttpMethod HttpMethod => new(Method);

  /// <summary>
  /// Gets or sets a string representation of the endpoint's path.
  /// </summary>
  public string Path { get; set; } = string.Empty;
  /// <summary>
  /// Gets the path of the endpoint.
  /// </summary>
  public Uri UriPath => new(Path, UriKind.RelativeOrAbsolute);
}
