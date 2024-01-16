namespace Logitar.Net.Http;

/// <summary>
/// Provides extension methods to help querying HTTP APIs.
/// </summary>
public static class HttpApiClientExtensions
{
  /// <summary>
  /// Sends a DELETE request to the specified end point.
  /// </summary>
  /// <param name="client">The HTTP API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API response.</returns>
  public static async Task<HttpApiResponse> DeleteAsync(this HttpApiClient client, Uri requestUri, CancellationToken cancellationToken = default)
  {
    return await client.SendAsync(HttpMethod.Delete, requestUri, cancellationToken);
  }
  /// <summary>
  /// Sends a DELETE request to the specified end point.
  /// </summary>
  /// <param name="client">The HTTP API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="context">The request context.</param>
  /// <returns>The API response.</returns>
  public static async Task<HttpApiResponse> DeleteAsync(this HttpApiClient client, Uri requestUri, HttpRequestContext context)
  {
    return await client.SendAsync(HttpMethod.Delete, requestUri, context);
  }

  /// <summary>
  /// Sends a GET request to the specified end point.
  /// </summary>
  /// <param name="client">The HTTP API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API response.</returns>
  public static async Task<HttpApiResponse> GetAsync(this HttpApiClient client, Uri requestUri, CancellationToken cancellationToken = default)
  {
    return await client.SendAsync(HttpMethod.Get, requestUri, cancellationToken);
  }
  /// <summary>
  /// Sends a GET request to the specified end point.
  /// </summary>
  /// <param name="client">The HTTP API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="context">The request context.</param>
  /// <returns>The API response.</returns>
  public static async Task<HttpApiResponse> GetAsync(this HttpApiClient client, Uri requestUri, HttpRequestContext context)
  {
    return await client.SendAsync(HttpMethod.Get, requestUri, context);
  }

  /// <summary>
  /// Sends a PATCH request to the specified end point.
  /// </summary>
  /// <param name="client">The HTTP API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API response.</returns>
  public static async Task<HttpApiResponse> PatchAsync(this HttpApiClient client, Uri requestUri, CancellationToken cancellationToken = default)
  {
    return await client.SendAsync(HttpMethod.Patch, requestUri, cancellationToken);
  }
  /// <summary>
  /// Sends a PATCH request to the specified end point.
  /// </summary>
  /// <param name="client">The HTTP API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="context">The request context.</param>
  /// <returns>The API response.</returns>
  public static async Task<HttpApiResponse> PatchAsync(this HttpApiClient client, Uri requestUri, HttpRequestContext context)
  {
    return await client.SendAsync(HttpMethod.Patch, requestUri, context);
  }
  /// <summary>
  /// Sends a PATCH request to the specified end point.
  /// </summary>
  /// <param name="client">The HTTP API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="requestContent">The HTTP request content.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API response.</returns>
  public static async Task<HttpApiResponse> PatchAsync(this HttpApiClient client, Uri requestUri, HttpContent? requestContent, CancellationToken cancellationToken = default)
  {
    return await client.SendAsync(HttpMethod.Patch, requestUri, requestContent, cancellationToken);
  }
  /// <summary>
  /// Sends a PATCH request to the specified end point.
  /// </summary>
  /// <param name="client">The HTTP API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="requestContent">The HTTP request content.</param>
  /// <param name="context">The request context.</param>
  /// <returns>The API response.</returns>
  public static async Task<HttpApiResponse> PatchAsync(this HttpApiClient client, Uri requestUri, HttpContent? requestContent, HttpRequestContext context)
  {
    return await client.SendAsync(HttpMethod.Patch, requestUri, requestContent, context);
  }

  /// <summary>
  /// Sends a POST request to the specified end point.
  /// </summary>
  /// <param name="client">The HTTP API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API response.</returns>
  public static async Task<HttpApiResponse> PostAsync(this HttpApiClient client, Uri requestUri, CancellationToken cancellationToken = default)
  {
    return await client.SendAsync(HttpMethod.Post, requestUri, cancellationToken);
  }
  /// <summary>
  /// Sends a POST request to the specified end point.
  /// </summary>
  /// <param name="client">The HTTP API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="context">The request context.</param>
  /// <returns>The API response.</returns>
  public static async Task<HttpApiResponse> PostAsync(this HttpApiClient client, Uri requestUri, HttpRequestContext context)
  {
    return await client.SendAsync(HttpMethod.Post, requestUri, context);
  }
  /// <summary>
  /// Sends a POST request to the specified end point.
  /// </summary>
  /// <param name="client">The HTTP API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="requestContent">The HTTP request content.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API response.</returns>
  public static async Task<HttpApiResponse> PostAsync(this HttpApiClient client, Uri requestUri, HttpContent? requestContent, CancellationToken cancellationToken = default)
  {
    return await client.SendAsync(HttpMethod.Post, requestUri, requestContent, cancellationToken);
  }
  /// <summary>
  /// Sends a POST request to the specified end point.
  /// </summary>
  /// <param name="client">The HTTP API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="requestContent">The HTTP request content.</param>
  /// <param name="context">The request context.</param>
  /// <returns>The API response.</returns>
  public static async Task<HttpApiResponse> PostAsync(this HttpApiClient client, Uri requestUri, HttpContent? requestContent, HttpRequestContext context)
  {
    return await client.SendAsync(HttpMethod.Post, requestUri, requestContent, context);
  }

  /// <summary>
  /// Sends a PUT request to the specified end point.
  /// </summary>
  /// <param name="client">The HTTP API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API response.</returns>
  public static async Task<HttpApiResponse> PutAsync(this HttpApiClient client, Uri requestUri, CancellationToken cancellationToken = default)
  {
    return await client.SendAsync(HttpMethod.Put, requestUri, cancellationToken);
  }
  /// <summary>
  /// Sends a PUT request to the specified end point.
  /// </summary>
  /// <param name="client">The HTTP API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="context">The request context.</param>
  /// <returns>The API response.</returns>
  public static async Task<HttpApiResponse> PutAsync(this HttpApiClient client, Uri requestUri, HttpRequestContext context)
  {
    return await client.SendAsync(HttpMethod.Put, requestUri, context);
  }
  /// <summary>
  /// Sends a PUT request to the specified end point.
  /// </summary>
  /// <param name="client">The HTTP API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="requestContent">The HTTP request content.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API response.</returns>
  public static async Task<HttpApiResponse> PutAsync(this HttpApiClient client, Uri requestUri, HttpContent? requestContent, CancellationToken cancellationToken = default)
  {
    return await client.SendAsync(HttpMethod.Put, requestUri, requestContent, cancellationToken);
  }
  /// <summary>
  /// Sends a PUT request to the specified end point.
  /// </summary>
  /// <param name="client">The HTTP API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="requestContent">The HTTP request content.</param>
  /// <param name="context">The request context.</param>
  /// <returns>The API response.</returns>
  public static async Task<HttpApiResponse> PutAsync(this HttpApiClient client, Uri requestUri, HttpContent? requestContent, HttpRequestContext context)
  {
    return await client.SendAsync(HttpMethod.Put, requestUri, requestContent, context);
  }
}
