namespace Logitar.Net.Http.Settings;

/// <summary>
/// Implements the settings of an HTTP API client.
/// </summary>
public record HttpApiSettings : IHttpApiSettings
{
  /// <summary>
  /// Gets or sets the authentication/authorization settings of the API.
  /// </summary>
  public IAuthorizationSettings? Authorization { get; set; }

  /// <summary>
  /// Gets or sets the base Uniform Resource Identifier (URI) of the API.
  /// </summary>
  public Uri? BaseUri { get; set; }

  /// <summary>
  /// Gets or sets the request headers.
  /// </summary>
  public List<HttpHeader> Headers { get; set; } = [];

  /// <summary>
  /// Gets a value indicating whether or not to throw an <see cref="HttpFailureException"/> when a HTTP response does not indicate success.
  /// </summary>
  public bool? ThrowOnFailure { get; set; }
}
