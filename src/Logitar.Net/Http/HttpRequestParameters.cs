namespace Logitar.Net.Http;

/// <summary>
/// Represents the parameters of an HTTP request.
/// </summary>
public record HttpRequestParameters : HttpRequestOptions
{
  /// <summary>
  /// Gets or sets the request method.
  /// </summary>
  public HttpMethod Method { get; set; } = HttpMethod.Get;

  /// <summary>
  /// Gets or sets the request Uniform Resource Identifier (URI).
  /// </summary>
  public Uri Uri { get; set; } = new(string.Empty, UriKind.RelativeOrAbsolute);

  /// <summary>
  /// Gets or sets a value indicating whether or not to throw an <see cref="HttpFailureException{T}"/> when an HTTP response does not indicate success.
  /// </summary>
  public bool? ThrowOnFailure { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpRequestParameters"/> class.
  /// </summary>
  public HttpRequestParameters()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpRequestParameters"/> class.
  /// </summary>
  /// <param name="method">The request method.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  public HttpRequestParameters(HttpMethod method, Uri uri)
  {
    Method = method;
    Uri = uri;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpRequestParameters"/> with the DELETE method.
  /// </summary>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <returns>The request parameters.</returns>
  public static HttpRequestParameters Delete(Uri uri) => new(HttpMethod.Delete, uri);
  /// <summary>
  /// Initializes a new instance of the <see cref="HttpRequestParameters"/> with the GET method.
  /// </summary>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <returns>The request parameters.</returns>
  public static HttpRequestParameters Get(Uri uri) => new(HttpMethod.Get, uri);
  /// <summary>
  /// Initializes a new instance of the <see cref="HttpRequestParameters"/> with the PATCH method.
  /// </summary>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <returns>The request parameters.</returns>
  public static HttpRequestParameters Patch(Uri uri) => new(HttpMethod.Patch, uri);
  /// <summary>
  /// Initializes a new instance of the <see cref="HttpRequestParameters"/> with the POST method.
  /// </summary>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <returns>The request parameters.</returns>
  public static HttpRequestParameters Post(Uri uri) => new(HttpMethod.Post, uri);
  /// <summary>
  /// Initializes a new instance of the <see cref="HttpRequestParameters"/> with the PUT method.
  /// </summary>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <returns>The request parameters.</returns>
  public static HttpRequestParameters Put(Uri uri) => new(HttpMethod.Put, uri);

  /// <summary>
  /// Applies the specified options to the parameters.
  /// </summary>
  /// <param name="options">The options to apply.</param>
  public void Apply(HttpRequestOptions? options)
  {
    if (options != null)
    {
      Content = options.Content;
      Headers.AddRange(options.Headers);
      Authorization = options.Authorization;
    }
  }
}
