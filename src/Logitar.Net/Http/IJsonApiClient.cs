namespace Logitar.Net.Http;

/// <summary>
/// Defines a client to query JSON APIs. JSON APIs are generally HTTP APIs that receive JSON payloads and return JSON data.
/// </summary>
public interface IJsonApiClient
{
  /// <summary>
  /// Sends a JSON request to the specified end point.
  /// </summary>
  /// <param name="method">The request HTTP method.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The HTTP API response.</returns>
  Task<JsonApiResponse<T>> SendAsync<T>(HttpMethod method, Uri requestUri, CancellationToken cancellationToken = default);
  /// <summary>
  /// Sends a JSON request to the specified end point.
  /// </summary>
  /// <param name="method">The HTTP request method.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="context">The HTTP request context.</param>
  /// <returns>The HTTP API response.</returns>
  Task<JsonApiResponse<T>> SendAsync<T>(HttpMethod method, Uri requestUri, HttpRequestContext context);
  /// <summary>
  /// Sends a JSON request to the specified end point.
  /// </summary>
  /// <param name="method">The HTTP request method.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="requestContent">The HTTP request content.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The HTTP API response.</returns>
  Task<JsonApiResponse<T>> SendAsync<T>(HttpMethod method, Uri requestUri, object? requestContent, CancellationToken cancellationToken = default);
  /// <summary>
  /// Sends a JSON request to the specified end point.
  /// </summary>
  /// <param name="method">The HTTP request method.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="requestContent">The HTTP request content.</param>
  /// <param name="context">The HTTP request context.</param>
  /// <returns>The HTTP API response.</returns>
  Task<JsonApiResponse<T>> SendAsync<T>(HttpMethod method, Uri requestUri, object? requestContent, HttpRequestContext context);
}
