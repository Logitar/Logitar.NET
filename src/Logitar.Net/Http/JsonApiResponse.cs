namespace Logitar.Net.Http;

/// <summary>
/// Represents the response of a JSON API.
/// </summary>
/// <typeparam name="T">The type of the response contents.</typeparam>
public record JsonApiResponse<T> : ApiResponse
{
  /// <summary>
  /// Gets or sets the text contents of the response, usually JSON.
  /// </summary>
  public string? ContentText
  {
    get => JsonSerializer.Serialize(Value, SerializerOptions);
    set => Value = value == null ? default : JsonSerializer.Deserialize<T>(value, SerializerOptions);
  }

  /// <summary>
  /// Gets or sets the value of the response.
  /// </summary>
  [JsonIgnore]
  public T? Value { get; set; }

  /// <summary>
  /// Gets or sets the serializer options.
  /// </summary>
  [JsonIgnore]
  public JsonSerializerOptions? SerializerOptions { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpApiResponse"/> class.
  /// </summary>
  public JsonApiResponse() : base()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpApiResponse"/> class.
  /// </summary>
  /// <param name="response">The HTTP response message.</param>
  public JsonApiResponse(HttpResponseMessage response) : base(response)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpApiResponse"/> class from the specified HTTP response message.
  /// </summary>
  /// <param name="response">The HTTP response message</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The JSON API response.</returns>
  public static async Task<JsonApiResponse<T>> FromResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken = default)
  {
    return await FromResponseAsync(response, serializerOptions: null, cancellationToken);
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="HttpApiResponse"/> class from the specified HTTP response message.
  /// </summary>
  /// <param name="response">The HTTP response message</param>
  /// <param name="serializerOptions">The serializer options.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The JSON API response.</returns>
  public static async Task<JsonApiResponse<T>> FromResponseAsync(HttpResponseMessage response, JsonSerializerOptions? serializerOptions, CancellationToken cancellationToken = default)
  {
    JsonApiResponse<T> result = new(response)
    {
      SerializerOptions = serializerOptions
    };

    try
    {
      result.ContentText = await response.Content.ReadAsStringAsync(cancellationToken);
    }
    catch (Exception)
    {
    }

    return result;
  }
}
