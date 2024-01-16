namespace Logitar.Net.Http;

/// <summary>
/// Represents the response of a JSON API.
/// </summary>
/// <typeparam name="T">The type of the response contents.</typeparam>
public record JsonApiResponse<T> : HttpApiResponse
{
  /// <summary>
  /// Gets or sets the text contents of the response.
  /// </summary>
  public override string? ContentText
  {
    get => base.ContentText;
    set
    {
      base.ContentText = value;

      if (typeof(T) == typeof(Empty) || value == null)
      {
        Value = default;
      }
      else
      {
        Value = JsonSerializer.Deserialize<T>(value, SerializerOptions);
      }
    }
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
}
