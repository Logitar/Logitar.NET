namespace Logitar.Net.Http;

/// <summary>
/// Defines a builder for HTTP requests.
/// </summary>
public interface IHttpRequestBuilder
{
  /// <summary>
  /// Gets the HTTP method.
  /// </summary>
  string Method { get; }
  /// <summary>
  /// Gets the Uniform Resource Locator (URL). It can contain placeholders using the format '{key}' that will be replaced by parameter values.
  /// </summary>
  string Url { get; }
  /// <summary>
  /// Gets the HTTP content.
  /// </summary>
  HttpContent? Content { get; }
  /// <summary>
  /// Gets the HTTP authorization.
  /// </summary>
  IHttpAuthorization? Authorization { get; }
  /// <summary>
  /// Gets the HTTP headers.
  /// </summary>
  IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; }

  /// <summary>
  /// Sets the Uniform Resource Locator (URL).
  /// </summary>
  /// <param name="url">The Uniform Resource Locator (URL).</param>
  /// <returns>The builder instance.</returns>
  IHttpRequestBuilder SetUrl(string url);
  /// <summary>
  /// Sets the Uniform Resource Identifier (URI). It can contain placeholders using the format '{key}' that will be replaced by parameter values.
  /// </summary>
  /// <param name="uri">The Uniform Resource Identifier (URI).</param>
  /// <returns>The builder instance.</returns>
  IHttpRequestBuilder SetUrl(Uri uri);

  /// <summary>
  /// Sets the HTTP method.
  /// </summary>
  /// <param name="method">The HTTP method.</param>
  /// <returns>The builder instance.</returns>
  IHttpRequestBuilder SetMethod(string method);
  /// <summary>
  /// Sets the HTTP method.
  /// </summary>
  /// <param name="method">The HTTP method.</param>
  /// <returns>The builder instance.</returns>
  IHttpRequestBuilder SetMethod(HttpMethod method);

  /// <summary>
  /// Sets the HTTP content.
  /// </summary>
  /// <param name="content">The HTTP content.</param>
  /// <returns>The builder instance.</returns>
  IHttpRequestBuilder SetContent(HttpContent? content);

  /// <summary>
  /// Sets a HTTP header.
  /// </summary>
  /// <param name="name">The name of the header.</param>
  /// <param name="value">The value of the header.</param>
  /// <returns>The builder instance.</returns>
  IHttpRequestBuilder SetHeader(string name, string value);
  /// <summary>
  /// Sets a HTTP header.
  /// </summary>
  /// <param name="name">The name of the header.</param>
  /// <param name="values">The values of the header.</param>
  /// <returns>The builder instance.</returns>
  IHttpRequestBuilder SetHeader(string name, IEnumerable<string> values);
  /// <summary>
  /// Sets a HTTP header.
  /// </summary>
  /// <param name="header">The HTTP header.</param>
  /// <returns>The builder instance.</returns>
  IHttpRequestBuilder SetHeader(HttpHeader header);
  /// <summary>
  /// Sets a list of HTTP headers.
  /// </summary>
  /// <param name="headers">The HTTP headers.</param>
  /// <returns>The builder instance.</returns>
  IHttpRequestBuilder SetHeaders(IEnumerable<HttpHeader> headers);

  /// <summary>
  /// Sets the BASIC authorization.
  /// </summary>
  /// <param name="identifier">The client identifier, such as an user name.</param>
  /// <param name="secret">The client secret, such as an user password.</param>
  /// <returns>The builder instance.</returns>
  IHttpRequestBuilder WithBasicAuthorization(string identifier, string secret);
  /// <summary>
  /// Sets the BASIC authorization.
  /// </summary>
  /// <param name="credentials">The client credentials.</param>
  /// <returns>The builder instance.</returns>
  IHttpRequestBuilder WithBasicAuthorization(ICredentials credentials);
  /// <summary>
  /// Sets the BASIC authorization.
  /// </summary>
  /// <param name="credentials">The base64-encoded credentials, using the '{identifier}:{secret}' format..</param>
  /// <returns>The builder instance.</returns>
  IHttpRequestBuilder WithBasicAuthorization(string credentials);
  /// <summary>
  /// Sets the BEARER authorization.
  /// </summary>
  /// <param name="credentials">The client credentials, such as an API key or a Bearer/Access token.</param>
  /// <returns>The builder instance.</returns>
  IHttpRequestBuilder WithBearerAuthorization(string credentials);
  /// <summary>
  /// Sets the request authorization.
  /// </summary>
  /// <param name="scheme">The authorization scheme.</param>
  /// <param name="credentials">The authorization credentials.</param>
  /// <returns>The builder instance.</returns>
  IHttpRequestBuilder WithAuthorization(string scheme, string credentials);
  /// <summary>
  /// Sets the request authorization.
  /// </summary>
  /// <param name="authorization">The authorization parameters.</param>
  /// <returns>The builder instance.</returns>
  IHttpRequestBuilder WithAuthorization(IHttpAuthorization? authorization);

  /// <summary>
  /// Applies the specified parameters to the request.
  /// </summary>
  /// <param name="parameters">The request parameters.</param>
  /// <returns>The builder instance.</returns>
  IHttpRequestBuilder WithParameters(HttpRequestParameters parameters);

  /// <summary>
  /// Builds an instance of the <see cref="HttpRequestMessage"/> class.
  /// </summary>
  /// <returns>The built instance.</returns>
  HttpRequestMessage BuildMessage();
}
