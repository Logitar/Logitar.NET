namespace Logitar.Net.Http;

/// <summary>
/// Defines a client to query HTTP APIs.
/// </summary>
public interface IHttpApiClient
{
  /// <summary>
  /// Sends an HTTP request to the specified end point.
  /// </summary>
  /// <param name="method">The request HTTP method.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The HTTP API response.</returns>
  Task<HttpApiResponse> SendAsync(HttpMethod method, Uri requestUri, CancellationToken cancellationToken = default);
  /// <summary>
  /// Sends an HTTP request to the specified end point.
  /// </summary>
  /// <param name="method">The HTTP request method.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="context">The HTTP request context.</param>
  /// <returns>The HTTP API response.</returns>
  Task<HttpApiResponse> SendAsync(HttpMethod method, Uri requestUri, HttpRequestContext context);
  /// <summary>
  /// Sends an HTTP request to the specified end point.
  /// </summary>
  /// <param name="method">The HTTP request method.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="requestContent">The HTTP request content.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The HTTP API response.</returns>
  Task<HttpApiResponse> SendAsync(HttpMethod method, Uri requestUri, HttpContent? requestContent, CancellationToken cancellationToken = default);
  /// <summary>
  /// Sends an HTTP request to the specified end point.
  /// </summary>
  /// <param name="method">The HTTP request method.</param>
  /// <param name="requestUri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="requestContent">The HTTP request content.</param>
  /// <param name="context">The HTTP request context.</param>
  /// <returns>The HTTP API response.</returns>
  Task<HttpApiResponse> SendAsync(HttpMethod method, Uri requestUri, HttpContent? requestContent, HttpRequestContext context);
}
