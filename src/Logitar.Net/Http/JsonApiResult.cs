namespace Logitar.Net.Http;

/// <summary>
/// Represents a JSON HTTP response.
/// </summary>
public record JsonApiResult : HttpApiResult
{
  /// <summary>
  /// Gets or sets the JSON content of the response.
  /// </summary>
  public string? JsonContent { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="JsonApiResult"/> class.
  /// </summary>
  public JsonApiResult() : base()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="JsonApiResult"/> class.
  /// </summary>
  /// <param name="response">The HTTP response message.</param>
  public JsonApiResult(HttpResponseMessage response) : base(response)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="JsonApiResult"/> class.
  /// </summary>
  /// <param name="response">The HTTP response.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The created instance.</returns>
  public static async Task<JsonApiResult> FromResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken = default)
  {
    JsonApiResult result = new(response);

    try
    {
      result.JsonContent = (await response.Content.ReadAsStringAsync(cancellationToken)).CleanTrim();
    }
    catch (Exception)
    {
    }

    return result;
  }

  /// <summary>
  /// Deserialies the result contents.
  /// </summary>
  /// <typeparam name="T">The type to deserialize into.</typeparam>
  /// <param name="options">The serialization options.</param>
  /// <returns>The deserialized instance.</returns>
  public T? Deserialize<T>(JsonSerializerOptions? options = null)
  {
    return JsonContent == null ? default : JsonSerializer.Deserialize<T>(JsonContent, options);
  }
}
