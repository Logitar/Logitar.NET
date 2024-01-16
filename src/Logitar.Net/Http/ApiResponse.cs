namespace Logitar.Net.Http;

/// <summary>
/// Represents the response of an API.
/// </summary>
public record ApiResponse
{
  /// <summary>
  /// Gets or sets the version of the response.
  /// </summary>
  public string Version { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the status of the response.
  /// </summary>
  public ApiStatus Status { get; set; } = new();

  /// <summary>
  /// Gets or sets the reason phrase of the response.
  /// </summary>
  public string? ReasonPhrase { get; set; }

  /// <summary>
  /// Gets or sets the headers of the response.
  /// </summary>
  public List<HttpHeader> Headers { get; set; } = [];

  /// <summary>
  /// Gets or sets the trailing headers of the response.
  /// </summary>
  public List<HttpHeader> TrailingHeaders { get; set; } = [];

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiResponse"/> class.
  /// </summary>
  public ApiResponse()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiResponse"/> class.
  /// </summary>
  /// <param name="response">The HTTP response message.</param>
  public ApiResponse(HttpResponseMessage response)
  {
    Version = response.Version.ToString();
    Status = new(response);
    ReasonPhrase = response.ReasonPhrase;

    foreach (KeyValuePair<string, IEnumerable<string>> header in response.Headers)
    {
      if (header.Value.Any())
      {
        Headers.Add(new HttpHeader(header.Key, header.Value));
      }
    }

    foreach (KeyValuePair<string, IEnumerable<string>> header in response.TrailingHeaders)
    {
      if (header.Value.Any())
      {
        TrailingHeaders.Add(new HttpHeader(header.Key, header.Value));
      }
    }
  }
}
