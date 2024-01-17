namespace Logitar.Net.Http;

/// <summary>
/// Defines the settings of an HTTP API client.
/// </summary>
public class HttpApiSettings : IHttpApiSettings
{
  /// <summary>
  /// Gets or sets the authentication/authorization settings of the API.
  /// </summary>
  public IHttpAuthorization? Authorization { get; set; }

  /// <summary>
  /// Gets or sets the base Uniform Resource Identifier (URI) of the API.
  /// </summary>
  public Uri? BaseUri { get; set; }

  /// <summary>
  /// Gets or sets the request headers.
  /// </summary>
  public List<HttpHeader> Headers { get; set; } = [];

  /// <summary>
  /// Gets or sets a value indicating whether or not to throw an <see cref="HttpFailureException{T}"/> when an HTTP response does not indicate success.
  /// </summary>
  public bool ThrowOnFailure { get; set; } = true;
}
