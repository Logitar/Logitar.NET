namespace Logitar.Net.Http;

/// <summary>
/// Provides extension methods for instances of the <see cref="HttpApiClient"/> class.
/// </summary>
public static class HttpApiClientExtensions
{
  /// <summary>
  /// Executes an HTTP DELETE request.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API result.</returns>
  public static async Task<HttpApiResult> DeleteAsync(this HttpApiClient client, Uri uri, CancellationToken cancellationToken)
  {
    return await client.DeleteAsync(uri, options: null, cancellationToken);
  }
  /// <summary>
  /// Executes an HTTP DELETE request.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="options">The request options.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API result.</returns>
  public static async Task<HttpApiResult> DeleteAsync(this HttpApiClient client, Uri uri, HttpRequestOptions? options, CancellationToken cancellationToken)
  {
    HttpRequestParameters parameters = HttpRequestParameters.Delete(uri);
    parameters.Apply(options);

    return await client.SendAsync(parameters, cancellationToken);
  }

  /// <summary>
  /// Executes an HTTP GET request.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API result.</returns>
  public static async Task<HttpApiResult> GetAsync(this HttpApiClient client, Uri uri, CancellationToken cancellationToken = default)
  {
    return await client.GetAsync(uri, options: null, cancellationToken);
  }
  /// <summary>
  /// Executes an HTTP GET request.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="options">The request options.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API result.</returns>
  public static async Task<HttpApiResult> GetAsync(this HttpApiClient client, Uri uri, HttpRequestOptions? options, CancellationToken cancellationToken = default)
  {
    HttpRequestParameters parameters = HttpRequestParameters.Get(uri);
    parameters.Apply(options);

    return await client.SendAsync(parameters, cancellationToken);
  }

  /// <summary>
  /// Executes an HTTP PATCH request.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API result.</returns>
  public static async Task<HttpApiResult> PatchAsync(this HttpApiClient client, Uri uri, CancellationToken cancellationToken = default)
  {
    return await client.PatchAsync(uri, options: null, cancellationToken);
  }
  /// <summary>
  /// Executes an HTTP PATCH request.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="options">The request options.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API result.</returns>
  public static async Task<HttpApiResult> PatchAsync(this HttpApiClient client, Uri uri, HttpRequestOptions? options, CancellationToken cancellationToken = default)
  {
    HttpRequestParameters parameters = HttpRequestParameters.Patch(uri);
    parameters.Apply(options);

    return await client.SendAsync(parameters, cancellationToken);
  }

  /// <summary>
  /// Executes an HTTP POST request.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API result.</returns>
  public static async Task<HttpApiResult> PostAsync(this HttpApiClient client, Uri uri, CancellationToken cancellationToken = default)
  {
    return await client.PostAsync(uri, options: null, cancellationToken);
  }
  /// <summary>
  /// Executes an HTTP POST request.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="options">The request options.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API result.</returns>
  public static async Task<HttpApiResult> PostAsync(this HttpApiClient client, Uri uri, HttpRequestOptions? options, CancellationToken cancellationToken = default)
  {
    HttpRequestParameters parameters = HttpRequestParameters.Post(uri);
    parameters.Apply(options);

    return await client.SendAsync(parameters, cancellationToken);
  }

  /// <summary>
  /// Executes an HTTP PUT request.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API result.</returns>
  public static async Task<HttpApiResult> PutAsync(this HttpApiClient client, Uri uri, CancellationToken cancellationToken = default)
  {
    return await client.PutAsync(uri, options: null, cancellationToken);
  }
  /// <summary>
  /// Executes an HTTP PUT request.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="options">The request options.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API result.</returns>
  public static async Task<HttpApiResult> PutAsync(this HttpApiClient client, Uri uri, HttpRequestOptions? options, CancellationToken cancellationToken = default)
  {
    HttpRequestParameters parameters = HttpRequestParameters.Put(uri);
    parameters.Apply(options);

    return await client.SendAsync(parameters, cancellationToken);
  }
}
