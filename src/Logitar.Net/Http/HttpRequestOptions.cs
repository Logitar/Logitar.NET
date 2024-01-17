namespace Logitar.Net.Http;

/// <summary>
/// Represents the optional parameters of an HTTP request.
/// </summary>
public record HttpRequestOptions
{
  /// <summary>
  /// Gets or sets the request contents.
  /// </summary>
  public HttpContent? Content { get; set; }

  /// <summary>
  /// Gets or sets the request headers.
  /// </summary>
  public List<HttpHeader> Headers { get; set; } = [];

  /// <summary>
  /// Gets or sets the request authorization parameters.
  /// </summary>
  public IHttpAuthorization? Authorization { get; set; }
}
