using Logitar.Net.Http.Settings;

namespace Logitar.Net.Http;

/// <summary>
/// Implements a client to query JSON APIs. JSON APIs are generally HTTP APIs that receive JSON payloads and return JSON data.
/// </summary>
public class JsonApiClient : HttpApiClient, IJsonApiClient
{
  /// <summary>
  /// Gets or sets the serializer options.
  /// </summary>
  protected JsonSerializerOptions? SerializerOptions { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="JsonApiClient"/> class.
  /// </summary>
  public JsonApiClient() : base()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="JsonApiClient"/> class.
  /// </summary>
  /// <param name="settings">The initialization settings.</param>
  public JsonApiClient(IJsonApiSettings settings) : base(settings)
  {
    Configure(settings);
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="JsonApiClient"/> class.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  public JsonApiClient(HttpClient client) : base(client)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="JsonApiClient"/> class.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  /// <param name="settings">The initialization settings.</param>
  public JsonApiClient(HttpClient client, IJsonApiSettings settings) : base(client, settings)
  {
    Configure(settings);
  }

  /// <summary>
  /// Sends a JSON request to the specified end point.
  /// </summary>
  /// <param name="method">The request HTTP method.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The HTTP API response.</returns>
  public virtual async Task<JsonApiResponse<T>> SendAsync<T>(HttpMethod method, Uri requestUri, CancellationToken cancellationToken)
  {
    return await SendAsync<T>(method, requestUri, requestContent: null, cancellationToken);
  }
  /// <summary>
  /// Sends a JSON request to the specified end point.
  /// </summary>
  /// <param name="method">The HTTP request method.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="context">The HTTP request context.</param>
  /// <returns>The HTTP API response.</returns>
  public virtual async Task<JsonApiResponse<T>> SendAsync<T>(HttpMethod method, Uri requestUri, HttpRequestContext context)
  {
    return await SendAsync<T>(method, requestUri, inputContent: null, context);
  }
  /// <summary>
  /// Sends a JSON request to the specified end point.
  /// </summary>
  /// <param name="method">The HTTP request method.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="requestContent">The HTTP request content.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The HTTP API response.</returns>
  public virtual async Task<JsonApiResponse<T>> SendAsync<T>(HttpMethod method, Uri requestUri, object? requestContent, CancellationToken cancellationToken)
  {
    HttpRequestContext context = new(cancellationToken);
    return await SendAsync<T>(method, requestUri, requestContent, context);
  }
  /// <summary>
  /// Sends a JSON request to the specified end point.
  /// </summary>
  /// <param name="method">The HTTP request method.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="inputContent">The HTTP request content.</param>
  /// <param name="context">The HTTP request context.</param>
  /// <returns>The HTTP API response.</returns>
  public virtual async Task<JsonApiResponse<T>> SendAsync<T>(HttpMethod method, Uri requestUri, object? inputContent, HttpRequestContext context)
  {
    JsonContent? requestContent = null;
    if (inputContent != null)
    {
      requestContent = JsonContent.Create(inputContent, inputContent.GetType());
    }

    HttpApiResponse httpResponse = await base.SendAsync(method, requestUri, requestContent, context);

    return new JsonApiResponse<T>
    {
      Version = httpResponse.Version,
      Status = httpResponse.Status,
      ReasonPhrase = httpResponse.ReasonPhrase,
      Headers = httpResponse.Headers,
      TrailingHeaders = httpResponse.TrailingHeaders,
      SerializerOptions = SerializerOptions,
      ContentText = httpResponse.ContentText
    };
  }

  /// <summary>
  /// Configures the JSON client.
  /// </summary>
  /// <param name="settings">The initialization settings.</param>
  protected virtual void Configure(IJsonApiSettings settings)
  {
    SerializerOptions = settings.SerializerOptions;
  }
}
