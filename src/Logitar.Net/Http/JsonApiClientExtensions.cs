namespace Logitar.Net.Http;

/// <summary>
/// Provides extension methods to help querying HTTP APIs.
/// </summary>
public static class JsonApiClientExtensions
{
  /// <summary>
  /// Sends a DELETE request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API response.</returns>
  public static async Task<JsonApiResponse<T>> DeleteAsync<T>(this JsonApiClient client, Uri requestUri, CancellationToken cancellationToken = default)
  {
    return await client.SendAsync<T>(HttpMethod.Delete, requestUri, cancellationToken);
  }
  /// <summary>
  /// Sends a DELETE request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="context">The request context.</param>
  /// <returns>The API response.</returns>
  public static async Task<JsonApiResponse<T>> DeleteAsync<T>(this JsonApiClient client, Uri requestUri, HttpRequestContext context)
  {
    return await client.SendAsync<T>(HttpMethod.Delete, requestUri, context);
  }

  /// <summary>
  /// Sends a GET request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API response.</returns>
  public static async Task<JsonApiResponse<T>> GetAsync<T>(this JsonApiClient client, Uri requestUri, CancellationToken cancellationToken = default)
  {
    return await client.SendAsync<T>(HttpMethod.Get, requestUri, cancellationToken);
  }
  /// <summary>
  /// Sends a GET request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="context">The request context.</param>
  /// <returns>The API response.</returns>
  public static async Task<JsonApiResponse<T>> GetAsync<T>(this JsonApiClient client, Uri requestUri, HttpRequestContext context)
  {
    return await client.SendAsync<T>(HttpMethod.Get, requestUri, context);
  }

  /// <summary>
  /// Sends a PATCH request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API response.</returns>
  public static async Task<JsonApiResponse<T>> PatchAsync<T>(this JsonApiClient client, Uri requestUri, CancellationToken cancellationToken = default)
  {
    return await client.SendAsync<T>(HttpMethod.Patch, requestUri, cancellationToken);
  }
  /// <summary>
  /// Sends a PATCH request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="context">The request context.</param>
  /// <returns>The API response.</returns>
  public static async Task<JsonApiResponse<T>> PatchAsync<T>(this JsonApiClient client, Uri requestUri, HttpRequestContext context)
  {
    return await client.SendAsync<T>(HttpMethod.Patch, requestUri, context);
  }
  /// <summary>
  /// Sends a PATCH request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="requestContent">The HTTP request content.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API response.</returns>
  public static async Task<JsonApiResponse<T>> PatchAsync<T>(this JsonApiClient client, Uri requestUri, object? requestContent, CancellationToken cancellationToken = default)
  {
    return await client.SendAsync<T>(HttpMethod.Patch, requestUri, requestContent, cancellationToken);
  }
  /// <summary>
  /// Sends a PATCH request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="requestContent">The HTTP request content.</param>
  /// <param name="context">The request context.</param>
  /// <returns>The API response.</returns>
  public static async Task<JsonApiResponse<T>> PatchAsync<T>(this JsonApiClient client, Uri requestUri, object? requestContent, HttpRequestContext context)
  {
    return await client.SendAsync<T>(HttpMethod.Patch, requestUri, requestContent, context);
  }

  /// <summary>
  /// Sends a POST request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API response.</returns>
  public static async Task<JsonApiResponse<T>> PostAsync<T>(this JsonApiClient client, Uri requestUri, CancellationToken cancellationToken = default)
  {
    return await client.SendAsync<T>(HttpMethod.Post, requestUri, cancellationToken);
  }
  /// <summary>
  /// Sends a POST request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="context">The request context.</param>
  /// <returns>The API response.</returns>
  public static async Task<JsonApiResponse<T>> PostAsync<T>(this JsonApiClient client, Uri requestUri, HttpRequestContext context)
  {
    return await client.SendAsync<T>(HttpMethod.Post, requestUri, context);
  }
  /// <summary>
  /// Sends a POST request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="requestContent">The HTTP request content.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API response.</returns>
  public static async Task<JsonApiResponse<T>> PostAsync<T>(this JsonApiClient client, Uri requestUri, object? requestContent, CancellationToken cancellationToken = default)
  {
    return await client.SendAsync<T>(HttpMethod.Post, requestUri, requestContent, cancellationToken);
  }
  /// <summary>
  /// Sends a POST request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="requestContent">The HTTP request content.</param>
  /// <param name="context">The request context.</param>
  /// <returns>The API response.</returns>
  public static async Task<JsonApiResponse<T>> PostAsync<T>(this JsonApiClient client, Uri requestUri, object? requestContent, HttpRequestContext context)
  {
    return await client.SendAsync<T>(HttpMethod.Post, requestUri, requestContent, context);
  }

  /// <summary>
  /// Sends a PUT request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API response.</returns>
  public static async Task<JsonApiResponse<T>> PutAsync<T>(this JsonApiClient client, Uri requestUri, CancellationToken cancellationToken = default)
  {
    return await client.SendAsync<T>(HttpMethod.Put, requestUri, cancellationToken);
  }
  /// <summary>
  /// Sends a PUT request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="context">The request context.</param>
  /// <returns>The API response.</returns>
  public static async Task<JsonApiResponse<T>> PutAsync<T>(this JsonApiClient client, Uri requestUri, HttpRequestContext context)
  {
    return await client.SendAsync<T>(HttpMethod.Put, requestUri, context);
  }
  /// <summary>
  /// Sends a PUT request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="requestContent">The HTTP request content.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API response.</returns>
  public static async Task<JsonApiResponse<T>> PutAsync<T>(this JsonApiClient client, Uri requestUri, object? requestContent, CancellationToken cancellationToken = default)
  {
    return await client.SendAsync<T>(HttpMethod.Put, requestUri, requestContent, cancellationToken);
  }
  /// <summary>
  /// Sends a PUT request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="requestContent">The HTTP request content.</param>
  /// <param name="context">The request context.</param>
  /// <returns>The API response.</returns>
  public static async Task<JsonApiResponse<T>> PutAsync<T>(this JsonApiClient client, Uri requestUri, object? requestContent, HttpRequestContext context)
  {
    return await client.SendAsync<T>(HttpMethod.Put, requestUri, requestContent, context);
  }
}
