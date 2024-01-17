namespace Logitar.Net.Http;

/// <summary>
/// Represents an abstraction of an HTTP API client.
/// </summary>
/// <typeparam name="T">The HTTP API result type.</typeparam>
public abstract class HttpApiClient<T> : IDisposable
{
  /// <summary>
  /// Gets or sets the HTTP client of this instance.
  /// </summary>
  protected HttpClient Client { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not to dispose the HTTP client when disposing this instance.
  /// </summary>
  protected bool DisposeClient { get; set; } = false;
  /// <summary>
  /// Gets or sets a value indicating whether or not to throw an <see cref="HttpFailureException{T}"/> when an HTTP response does not indicate success.
  /// </summary>
  protected bool ThrowOnFailure { get; set; } = true;

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpApiClient{T}"/> class.
  /// An instance of the <see cref="HttpClient"/> class will be created; this instance shall be disposed explicitly.
  /// </summary>
  protected HttpApiClient() : this(new HttpClient())
  {
    DisposeClient = true;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpApiClient{T}"/> class.
  /// An instance of the <see cref="HttpClient"/> class will be created; this instance shall be disposed explicitly.
  /// </summary>
  /// <param name="settings">The HTTP API settings.</param>
  protected HttpApiClient(IHttpApiSettings settings) : this()
  {
    Apply(settings);
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpApiClient{T}"/> class.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  protected HttpApiClient(HttpClient client)
  {
    Client = client;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpApiClient{T}"/> class.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  /// <param name="settings">The HTTP API settings.</param>
  protected HttpApiClient(HttpClient client, IHttpApiSettings settings) : this(client)
  {
    Apply(settings);
  }

  /// <summary>
  /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
  /// </summary>
  public void Dispose()
  {
    if (DisposeClient)
    {
      Client.Dispose();
    }

    GC.SuppressFinalize(this);
  }

  /// <summary>
  /// Sends the specified request to a remote API and returns the result.
  /// </summary>
  /// <param name="parameters">The HTTP request parameters.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API result.</returns>
  public virtual async Task<T> SendAsync(HttpRequestParameters parameters, CancellationToken cancellationToken)
  {
    using HttpRequestMessage request = await BuildRequestAsync(parameters, cancellationToken);
    using HttpResponseMessage response = await Client.SendAsync(request, cancellationToken);

    T result = await BuildResultAsync(response, cancellationToken);

    if (parameters.ThrowOnFailure ?? ThrowOnFailure)
    {
      response.ThrowOnFailure(result);
    }

    return result;
  }

  /// <summary>
  /// Builds an HTTP request from the specified parameters.
  /// </summary>
  /// <param name="parameters">The request parameters.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The built request.</returns>
  protected virtual Task<HttpRequestMessage> BuildRequestAsync(HttpRequestParameters parameters, CancellationToken cancellationToken)
  {
    return Task.FromResult(HttpRequestBuilder.BuildMessage(parameters));
  }

  /// <summary>
  /// Builds an API result from the specified response.
  /// </summary>
  /// <param name="response">The HTTP response.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The built result.</returns>
  protected abstract Task<T> BuildResultAsync(HttpResponseMessage response, CancellationToken cancellationToken);

  /// <summary>
  /// Applies the specified HTTP API settings.
  /// </summary>
  /// <param name="settings">The HTTP API settings.</param>
  protected virtual void Apply(IHttpApiSettings settings)
  {
    ThrowOnFailure = settings.ThrowOnFailure;

    Client.Apply(settings);
  }
}
