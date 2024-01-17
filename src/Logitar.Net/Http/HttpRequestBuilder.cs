namespace Logitar.Net.Http;

/// <summary>
/// Implements a builder for HTTP requests.
/// </summary>
public class HttpRequestBuilder : IHttpRequestBuilder
{
  /// <summary>
  /// Gets or sets the HTTP method.
  /// </summary>
  public virtual string Method { get; protected set; }
  /// <summary>
  /// Gets or sets the Uniform Resource Locator (URL).
  /// </summary>
  public virtual string Url { get; protected set; }
  /// <summary>
  /// Gets the HTTP content.
  /// </summary>
  public virtual HttpContent? Content { get; protected set; }
  /// <summary>
  /// Gets the HTTP authorization.
  /// </summary>
  public virtual IHttpAuthorization? Authorization { get; protected set; }
  /// <summary>
  /// Gets the HTTP headers.
  /// </summary>
  public virtual IReadOnlyDictionary<string, IEnumerable<string>> Headers => RequestHeaders.AsReadOnly();

  /// <summary>
  /// Gets or sets the request headers.
  /// </summary>
  protected Dictionary<string, IEnumerable<string>> RequestHeaders { get; set; } = [];

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpRequestBuilder"/> class.
  /// </summary>
  public HttpRequestBuilder() : this(HttpMethod.Get, new Uri(string.Empty, UriKind.RelativeOrAbsolute))
  {
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="HttpRequestBuilder"/> class.
  /// </summary>
  /// <param name="method">The HTTP method.</param>
  /// <param name="uri">The Uniform Resource Identifier (URI).</param>
  public HttpRequestBuilder(HttpMethod method, Uri uri) : this(method.Method, uri.ToString())
  {
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="HttpRequestBuilder"/> class.
  /// </summary>
  /// <param name="method">The HTTP method.</param>
  /// <param name="url">The Uniform Resource Locator (URL).</param>
  public HttpRequestBuilder(string method, string url)
  {
    Method = method;
    Url = url;
  }

  /// <summary>
  /// Initalizes a new instance of the <see cref="HttpRequestBuilder"/> class using the DELETE method.
  /// </summary>
  /// <param name="url">The Uniform Resource Locator (URL).</param>
  /// <returns>The builder instance.</returns>
  public static HttpRequestBuilder Delete(string url) => new(HttpMethod.Delete.Method, url);
  /// <summary>
  /// Initalizes a new instance of the <see cref="HttpRequestBuilder"/> class using the DELETE method.
  /// </summary>
  /// <param name="uri">The Uniform Resource Identifier (URI).</param>
  /// <returns>The builder instance.</returns>
  public static HttpRequestBuilder Delete(Uri uri) => new(HttpMethod.Delete, uri);
  /// <summary>
  /// Initalizes a new instance of the <see cref="HttpRequestBuilder"/> class using the GET method.
  /// </summary>
  /// <param name="url">The Uniform Resource Locator (URL).</param>
  /// <returns>The builder instance.</returns>
  public static HttpRequestBuilder Get(string url) => new(HttpMethod.Get.Method, url);
  /// <summary>
  /// Initalizes a new instance of the <see cref="HttpRequestBuilder"/> class using the GET method.
  /// </summary>
  /// <param name="uri">The Uniform Resource Identifier (URI).</param>
  /// <returns>The builder instance.</returns>
  public static HttpRequestBuilder Get(Uri uri) => new(HttpMethod.Get, uri);
  /// <summary>
  /// Initalizes a new instance of the <see cref="HttpRequestBuilder"/> class using the PATCH method.
  /// </summary>
  /// <param name="url">The Uniform Resource Locator (URL).</param>
  /// <returns>The builder instance.</returns>
  public static HttpRequestBuilder Patch(string url) => new(HttpMethod.Patch.Method, url);
  /// <summary>
  /// Initalizes a new instance of the <see cref="HttpRequestBuilder"/> class using the PATCH method.
  /// </summary>
  /// <param name="uri">The Uniform Resource Identifier (URI).</param>
  /// <returns>The builder instance.</returns>
  public static HttpRequestBuilder Patch(Uri uri) => new(HttpMethod.Patch, uri);
  /// <summary>
  /// Initalizes a new instance of the <see cref="HttpRequestBuilder"/> class using the POST method.
  /// </summary>
  /// <param name="url">The Uniform Resource Locator (URL).</param>
  /// <returns>The builder instance.</returns>
  public static HttpRequestBuilder Post(string url) => new(HttpMethod.Post.Method, url);
  /// <summary>
  /// Initalizes a new instance of the <see cref="HttpRequestBuilder"/> class using the POST method.
  /// </summary>
  /// <param name="uri">The Uniform Resource Identifier (URI).</param>
  /// <returns>The builder instance.</returns>
  public static HttpRequestBuilder Post(Uri uri) => new(HttpMethod.Post, uri);
  /// <summary>
  /// Initalizes a new instance of the <see cref="HttpRequestBuilder"/> class using the PUT method.
  /// </summary>
  /// <param name="url">The Uniform Resource Locator (URL).</param>
  /// <returns>The builder instance.</returns>
  public static HttpRequestBuilder Put(string url) => new(HttpMethod.Put.Method, url);
  /// <summary>
  /// Initalizes a new instance of the <see cref="HttpRequestBuilder"/> class using the PUT method.
  /// </summary>
  /// <param name="uri">The Uniform Resource Identifier (URI).</param>
  /// <returns>The builder instance.</returns>
  public static HttpRequestBuilder Put(Uri uri) => new(HttpMethod.Put, uri);

  /// <summary>
  /// Sets the Uniform Resource Locator (URL).
  /// </summary>
  /// <param name="url">The Uniform Resource Locator (URL).</param>
  /// <returns>The builder instance.</returns>
  public virtual IHttpRequestBuilder SetUrl(string url)
  {
    Url = url;
    return this;
  }
  /// <summary>
  /// Sets the Uniform Resource Identifier (URI). It can contain placeholders using the format '{key}' that will be replaced by parameter values.
  /// </summary>
  /// <param name="uri">The Uniform Resource Identifier (URI).</param>
  /// <returns>The builder instance.</returns>
  public virtual IHttpRequestBuilder SetUrl(Uri uri) => SetUrl(uri.ToString());

  /// <summary>
  /// Sets the HTTP method.
  /// </summary>
  /// <param name="method">The HTTP method.</param>
  /// <returns>The builder instance.</returns>
  public virtual IHttpRequestBuilder SetMethod(HttpMethod method) => SetMethod(method.Method);
  /// <summary>
  /// Sets the HTTP method.
  /// </summary>
  /// <param name="method">The HTTP method.</param>
  /// <returns>The builder instance.</returns>
  public virtual IHttpRequestBuilder SetMethod(string method)
  {
    Method = method;
    return this;
  }


  /// <summary>
  /// Sets the HTTP content.
  /// </summary>
  /// <param name="content">The HTTP content.</param>
  /// <returns>The builder instance.</returns>
  public virtual IHttpRequestBuilder SetContent(HttpContent? content)
  {
    Content = content;
    return this;
  }

  /// <summary>
  /// Sets a HTTP header.
  /// </summary>
  /// <param name="name">The name of the header.</param>
  /// <param name="value">The value of the header.</param>
  /// <returns>The builder instance.</returns>
  public virtual IHttpRequestBuilder SetHeader(string name, string value) => SetHeader(name, [value]);
  /// <summary>
  /// Sets a HTTP header.
  /// </summary>
  /// <param name="name">The name of the header.</param>
  /// <param name="values">The values of the header.</param>
  /// <returns>The builder instance.</returns>
  public virtual IHttpRequestBuilder SetHeader(string name, IEnumerable<string> values)
  {
    RequestHeaders[name] = values;
    return this;
  }
  /// <summary>
  /// Sets a HTTP header.
  /// </summary>
  /// <param name="header">The HTTP header.</param>
  /// <returns>The builder instance.</returns>
  public virtual IHttpRequestBuilder SetHeader(HttpHeader header) => SetHeader(header.Name, header.Values);

  /// <summary>
  /// Sets a list of HTTP headers.
  /// </summary>
  /// <param name="headers">The HTTP headers.</param>
  /// <returns>The builder instance.</returns>
  public virtual IHttpRequestBuilder SetHeaders(IEnumerable<HttpHeader> headers)
  {
    foreach (HttpHeader header in headers)
    {
      SetHeader(header);
    }
    return this;
  }

  /// <summary>
  /// Sets the BASIC authorization.
  /// </summary>
  /// <param name="identifier">The client identifier, such as an user name.</param>
  /// <param name="secret">The client secret, such as an user password.</param>
  /// <returns>The builder instance.</returns>
  public virtual IHttpRequestBuilder WithBasicAuthorization(string identifier, string secret) => WithAuthorization(HttpAuthorization.Basic(identifier, secret));
  /// <summary>
  /// Sets the BASIC authorization.
  /// </summary>
  /// <param name="credentials">The client credentials.</param>
  /// <returns>The builder instance.</returns>
  public virtual IHttpRequestBuilder WithBasicAuthorization(ICredentials credentials) => WithAuthorization(HttpAuthorization.Basic(credentials));
  /// <summary>
  /// Sets the BASIC authorization.
  /// </summary>
  /// <param name="credentials">The base64-encoded credentials, using the '{identifier}:{secret}' format..</param>
  /// <returns>The builder instance.</returns>
  public virtual IHttpRequestBuilder WithBasicAuthorization(string credentials) => WithAuthorization(HttpAuthorization.Basic(credentials));
  /// <summary>
  /// Sets the BEARER authorization.
  /// </summary>
  /// <param name="credentials">The client credentials, such as an API key or a Bearer/Access token.</param>
  /// <returns>The builder instance.</returns>
  public virtual IHttpRequestBuilder WithBearerAuthorization(string credentials) => WithAuthorization(HttpAuthorization.Bearer(credentials));
  /// <summary>
  /// Sets the request authorization.
  /// </summary>
  /// <param name="scheme">The authorization scheme.</param>
  /// <param name="credentials">The authorization credentials.</param>
  /// <returns>The builder instance.</returns>
  public virtual IHttpRequestBuilder WithAuthorization(string scheme, string credentials) => WithAuthorization(new HttpAuthorization(scheme, credentials));
  /// <summary>
  /// Sets the request authorization.
  /// </summary>
  /// <param name="authorization">The authorization parameters.</param>
  /// <returns>The builder instance.</returns>
  public virtual IHttpRequestBuilder WithAuthorization(IHttpAuthorization? authorization)
  {
    Authorization = authorization;
    return this;
  }

  /// <summary>
  /// Builds an instance of the <see cref="HttpRequestMessage"/> class.
  /// </summary>
  /// <returns>The built instance.</returns>
  public virtual HttpRequestMessage BuildMessage()
  {
    HttpMethod method = new(Method);
    Uri requestUri = new(Url, UriKind.RelativeOrAbsolute);
    HttpRequestMessage request = new(method, requestUri)
    {
      Content = Content
    };

    if (Authorization != null)
    {
      request.Headers.Authorization = new AuthenticationHeaderValue(Authorization.Scheme, Authorization.Credentials);
    }

    foreach (KeyValuePair<string, IEnumerable<string>> header in RequestHeaders)
    {
      request.Headers.Add(header.Key, header.Value);
    }

    return request;
  }
}
