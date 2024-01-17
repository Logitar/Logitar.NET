namespace Logitar.Net.Http;

/// <summary>
/// Represents the parameters of an HTTP request.
/// </summary>
public record HttpRequestParameters : HttpRequestOptions
{
  /// <summary>
  /// Gets or sets the request method.
  /// </summary>
  public HttpMethod Method { get; set; } = HttpMethod.Get;

  /// <summary>
  /// Gets or sets the request Uniform Resource Identifier (URI).
  /// </summary>
  public Uri Uri { get; set; } = new(string.Empty, UriKind.RelativeOrAbsolute);

  /// <summary>
  /// Gets or sets a value indicating whether or not to throw an <see cref="HttpFailureException"/> when an HTTP response does not indicate success.
  /// </summary>
  public bool? ThrowOnFailure { get; set; }
}
