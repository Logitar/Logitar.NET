using Logitar.Net.Http.Settings;

namespace Logitar.Net.Http;

/// <summary>
/// Implements a client to query HTTP APIs.
/// </summary>
public class HttpApiClient : IDisposable, IHttpApiClient
{
  /// <summary>
  /// Gets the HTTP client of this instance.
  /// </summary>
  protected HttpClient Client { get; }
  /// <summary>
  /// Gets or sets a value indicating whether or not to dispose the HTTP client when disposing this instance.
  /// </summary>
  protected bool DisposeClient { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not to throw an <see cref="HttpFailureException"/> when a HTTP response does not indicate success.
  /// </summary>
  protected bool? ThrowOnFailure { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpApiClient"/> class.
  /// An instance of the <see cref="HttpClient"/> will be created, so this instance shall be disposed explicitly.
  /// </summary>
  public HttpApiClient() : this(new HttpClient())
  {
    DisposeClient = true;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpApiClient"/> class.
  /// An instance of the <see cref="HttpClient"/> will be created, so this instance shall be disposed explicitly.
  /// </summary>
  /// <param name="settings">The initialization settings.</param>
  public HttpApiClient(IHttpApiSettings settings) : this()
  {
    Configure(settings);
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpApiClient"/> class.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  public HttpApiClient(HttpClient client)
  {
    Client = client;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpApiClient"/> class.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  /// <param name="settings">The initialization settings.</param>
  public HttpApiClient(HttpClient client, IHttpApiSettings settings) : this(client)
  {
    Configure(settings);
  }

  /// <summary>
  /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
  /// </summary>
  public virtual void Dispose()
  {
    if (DisposeClient)
    {
      Client.Dispose();
    }

    GC.SuppressFinalize(this);
  }

  /// <summary>
  /// Sends an HTTP request to the specified end point.
  /// </summary>
  /// <param name="method">The request HTTP method.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The HTTP API response.</returns>
  public virtual async Task<HttpApiResponse> SendAsync(HttpMethod method, Uri requestUri, CancellationToken cancellationToken)
  {
    return await SendAsync(method, requestUri, requestContent: null, cancellationToken);
  }
  /// <summary>
  /// Sends an HTTP request to the specified end point.
  /// </summary>
  /// <param name="method">The HTTP request method.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="context">The HTTP request context.</param>
  /// <returns>The HTTP API response.</returns>
  public virtual async Task<HttpApiResponse> SendAsync(HttpMethod method, Uri requestUri, HttpRequestContext context)
  {
    return await SendAsync(method, requestUri, requestContent: null, context);
  }
  /// <summary>
  /// Sends an HTTP request to the specified end point.
  /// </summary>
  /// <param name="method">The HTTP request method.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="requestContent">The HTTP request content.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The HTTP API response.</returns>
  public virtual async Task<HttpApiResponse> SendAsync(HttpMethod method, Uri requestUri, HttpContent? requestContent, CancellationToken cancellationToken)
  {
    HttpRequestContext context = new(cancellationToken);
    return await SendAsync(method, requestUri, requestContent, context);
  }
  /// <summary>
  /// Sends an HTTP request to the specified end point.
  /// </summary>
  /// <param name="method">The HTTP request method.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="requestContent">The HTTP request content.</param>
  /// <param name="context">The HTTP request context.</param>
  /// <returns>The HTTP API response.</returns>
  public virtual async Task<HttpApiResponse> SendAsync(HttpMethod method, Uri requestUri, HttpContent? requestContent, HttpRequestContext context)
  {
    using HttpRequestMessage request = new(method, requestUri)
    {
      Content = requestContent
    };
    foreach (HttpHeader header in context.Headers)
    {
      request.Headers.Add(header.Name, header.Values);
    }
    if (context.Authorization != null)
    {
      IAuthorizationSettings authorization = context.Authorization;
      request.Headers.Authorization = new AuthenticationHeaderValue(authorization.Scheme, authorization.Credentials);
    }

    using HttpResponseMessage response = await Client.SendAsync(request, context.CancellationToken);
    HttpApiResponse result = new(response);

    if (ThrowOnFailure ?? context.ThrowOnFailure)
    {
      try
      {
        response.EnsureSuccessStatusCode();
      }
      catch (Exception innerException)
      {
        throw new HttpFailureException(result, innerException);
      }
    }

    return new HttpApiResponse(response);
  }

  /// <summary>
  /// Configures the HTTP client.
  /// </summary>
  /// <param name="settings">The initialization settings.</param>
  protected virtual void Configure(IHttpApiSettings settings)
  {
    Client.BaseAddress = settings.BaseUri;

    foreach (HttpHeader header in settings.Headers)
    {
      Client.DefaultRequestHeaders.Add(header.Name, header.Values);
    }

    if (settings.Authorization != null)
    {
      IAuthorizationSettings authorization = settings.Authorization;
      Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authorization.Scheme, authorization.Credentials);
    }

    ThrowOnFailure = settings.ThrowOnFailure;
  }
}
