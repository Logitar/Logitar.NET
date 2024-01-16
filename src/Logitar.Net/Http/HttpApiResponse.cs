namespace Logitar.Net.Http;

/// <summary>
/// Represents the response of an HTTP API.
/// </summary>
public record HttpApiResponse : ApiResponse
{
  /// <summary>
  /// Gets or sets the contents of the response.
  /// </summary>
  [JsonIgnore]
  public HttpContent? Content { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpApiResponse"/> class.
  /// </summary>
  public HttpApiResponse() : base()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpApiResponse"/> class.
  /// </summary>
  /// <param name="response">The HTTP response message.</param>
  public HttpApiResponse(HttpResponseMessage response) : base(response)
  {
    Content = response.Content;
  }
}
