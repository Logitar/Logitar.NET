namespace Logitar.Net.Http;

/// <summary>
/// Represents a simple HTTP response.
/// </summary>
public record HttpApiResult
{
  /// <summary>
  /// Gets or sets the response version.
  /// </summary>
  public Version Version { get; set; } = new();
  /// <summary>
  /// Gets or sets the response status.
  /// </summary>
  public HttpStatus Status { get; set; } = new();
  /// <summary>
  /// Gets or sets the reason phrase of the response.
  /// </summary>
  public string? ReasonPhrase { get; set; }
  /// <summary>
  /// Gets or sets the response headers.
  /// </summary>
  public List<HttpHeader> Headers { get; set; } = [];
  /// <summary>
  /// Gets or sets the response trailing headers.
  /// </summary>
  public List<HttpHeader> TrailingHeaders { get; set; } = [];

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpApiResult"/> class.
  /// </summary>
  public HttpApiResult()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpApiResult"/> class.
  /// </summary>
  /// <param name="response">The HTTP response message.</param>
  public HttpApiResult(HttpResponseMessage response)
  {
    Version = response.Version;
    Status = new HttpStatus(response);
    ReasonPhrase = response.ReasonPhrase;

    foreach (KeyValuePair<string, IEnumerable<string>> header in response.Headers)
    {
      Headers.Add(new HttpHeader(header.Key, header.Value));
    }

    foreach (KeyValuePair<string, IEnumerable<string>> header in response.TrailingHeaders)
    {
      TrailingHeaders.Add(new HttpHeader(header.Key, header.Value));
    }
  }
}
