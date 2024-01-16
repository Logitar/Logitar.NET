namespace Logitar.Net.Http;

/// <summary>
/// Provides extension methods to help querying HTTP APIs.
/// </summary>
public static class EmptyJsonApiClientExtensions
{
  /// <summary>
  /// Sends a DELETE request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API response.</returns>
  public static async Task<ApiResponse> DeleteAsync(this JsonApiClient client, Uri requestUri, CancellationToken cancellationToken = default)
  {
    return await client.SendAsync<Empty>(HttpMethod.Delete, requestUri, cancellationToken);
  }
  /// <summary>
  /// Sends a DELETE request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="context">The request context.</param>
  /// <returns>The API response.</returns>
  public static async Task<ApiResponse> DeleteAsync(this JsonApiClient client, Uri requestUri, HttpRequestContext context)
  {
    return await client.SendAsync<Empty>(HttpMethod.Delete, requestUri, context);
  }

  /// <summary>
  /// Sends a GET request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API response.</returns>
  public static async Task<ApiResponse> GetAsync(this JsonApiClient client, Uri requestUri, CancellationToken cancellationToken = default)
  {
    return await client.SendAsync<Empty>(HttpMethod.Get, requestUri, cancellationToken);
  }
  /// <summary>
  /// Sends a GET request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="context">The request context.</param>
  /// <returns>The API response.</returns>
  public static async Task<ApiResponse> GetAsync(this JsonApiClient client, Uri requestUri, HttpRequestContext context)
  {
    return await client.SendAsync<Empty>(HttpMethod.Get, requestUri, context);
  }

  /// <summary>
  /// Sends a PATCH request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API response.</returns>
  public static async Task<ApiResponse> PatchAsync(this JsonApiClient client, Uri requestUri, CancellationToken cancellationToken = default)
  {
    return await client.SendAsync<Empty>(HttpMethod.Patch, requestUri, cancellationToken);
  }
  /// <summary>
  /// Sends a PATCH request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="context">The request context.</param>
  /// <returns>The API response.</returns>
  public static async Task<ApiResponse> PatchAsync(this JsonApiClient client, Uri requestUri, HttpRequestContext context)
  {
    return await client.SendAsync<Empty>(HttpMethod.Patch, requestUri, context);
  }
  /// <summary>
  /// Sends a PATCH request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="requestContent">The HTTP request content.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API response.</returns>
  public static async Task<ApiResponse> PatchAsync(this JsonApiClient client, Uri requestUri, object? requestContent, CancellationToken cancellationToken = default)
  {
    return await client.SendAsync<Empty>(HttpMethod.Patch, requestUri, requestContent, cancellationToken);
  }
  /// <summary>
  /// Sends a PATCH request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="requestContent">The HTTP request content.</param>
  /// <param name="context">The request context.</param>
  /// <returns>The API response.</returns>
  public static async Task<ApiResponse> PatchAsync(this JsonApiClient client, Uri requestUri, object? requestContent, HttpRequestContext context)
  {
    return await client.SendAsync<Empty>(HttpMethod.Patch, requestUri, requestContent, context);
  }

  /// <summary>
  /// Sends a POST request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API response.</returns>
  public static async Task<ApiResponse> PostAsync(this JsonApiClient client, Uri requestUri, CancellationToken cancellationToken = default)
  {
    return await client.SendAsync<Empty>(HttpMethod.Post, requestUri, cancellationToken);
  }
  /// <summary>
  /// Sends a POST request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="context">The request context.</param>
  /// <returns>The API response.</returns>
  public static async Task<ApiResponse> PostAsync(this JsonApiClient client, Uri requestUri, HttpRequestContext context)
  {
    return await client.SendAsync<Empty>(HttpMethod.Post, requestUri, context);
  }
  /// <summary>
  /// Sends a POST request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="requestContent">The HTTP request content.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API response.</returns>
  public static async Task<ApiResponse> PostAsync(this JsonApiClient client, Uri requestUri, object? requestContent, CancellationToken cancellationToken = default)
  {
    return await client.SendAsync<Empty>(HttpMethod.Post, requestUri, requestContent, cancellationToken);
  }
  /// <summary>
  /// Sends a POST request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="requestContent">The HTTP request content.</param>
  /// <param name="context">The request context.</param>
  /// <returns>The API response.</returns>
  public static async Task<ApiResponse> PostAsync(this JsonApiClient client, Uri requestUri, object? requestContent, HttpRequestContext context)
  {
    return await client.SendAsync<Empty>(HttpMethod.Post, requestUri, requestContent, context);
  }

  /// <summary>
  /// Sends a PUT request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API response.</returns>
  public static async Task<ApiResponse> PutAsync(this JsonApiClient client, Uri requestUri, CancellationToken cancellationToken = default)
  {
    return await client.SendAsync<Empty>(HttpMethod.Put, requestUri, cancellationToken);
  }
  /// <summary>
  /// Sends a PUT request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="context">The request context.</param>
  /// <returns>The API response.</returns>
  public static async Task<ApiResponse> PutAsync(this JsonApiClient client, Uri requestUri, HttpRequestContext context)
  {
    return await client.SendAsync<Empty>(HttpMethod.Put, requestUri, context);
  }
  /// <summary>
  /// Sends a PUT request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="requestContent">The HTTP request content.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API response.</returns>
  public static async Task<ApiResponse> PutAsync(this JsonApiClient client, Uri requestUri, object? requestContent, CancellationToken cancellationToken = default)
  {
    return await client.SendAsync<Empty>(HttpMethod.Put, requestUri, requestContent, cancellationToken);
  }
  /// <summary>
  /// Sends a PUT request to the specified end point.
  /// </summary>
  /// <param name="client">The JSON API client.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="requestContent">The HTTP request content.</param>
  /// <param name="context">The request context.</param>
  /// <returns>The API response.</returns>
  public static async Task<ApiResponse> PutAsync(this JsonApiClient client, Uri requestUri, object? requestContent, HttpRequestContext context)
  {
    return await client.SendAsync<Empty>(HttpMethod.Put, requestUri, requestContent, context);
  }
}
