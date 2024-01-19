namespace Logitar.Net.Http;

/// <summary>
/// Represents the optional parameters of an HTTP request to a JSON API.
/// </summary>
public record JsonRequestOptions : HttpRequestOptions
{
  /// <summary>
  /// Gets or sets the JSON serializer options.
  /// </summary>
  public JsonSerializerOptions? SerializerOptions { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="JsonRequestOptions"/> class.
  /// </summary>
  public JsonRequestOptions() : this(content: null)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="JsonRequestOptions"/> class.
  /// </summary>
  /// <param name="content">The request content.</param>
  public JsonRequestOptions(object? content) : base()
  {
    SetContentValue(content);
  }

  /// <summary>
  /// Gets the typed content value.
  /// </summary>
  /// <typeparam name="T">The value type.</typeparam>
  /// <returns>The content value.</returns>
  public T? GetContentValue<T>()
  {
    return Content is JsonContent jsonContent ? (T?)jsonContent.Value : default;
  }
  /// <summary>
  /// Sets the content value.
  /// </summary>
  /// <param name="value">The value.</param>
  /// <param name="options">The serialization options.</param>
  public void SetContentValue(object? value, JsonSerializerOptions? options = null)
  {
    Content = value == null ? null : JsonContent.Create(value, value.GetType(), options: options);
  }
}
