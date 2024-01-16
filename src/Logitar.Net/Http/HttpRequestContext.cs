using Logitar.Net.Http.Settings;

namespace Logitar.Net.Http;

/// <summary>
/// Represents the context of an HTTP request.
/// </summary>
public record HttpRequestContext
{
  /// <summary>
  /// Gets or sets the authorization settings.
  /// </summary>
  public IAuthorizationSettings? Authorization { get; set; }

  /// <summary>
  /// Gets or sets the cancellation token.
  /// </summary>
  public CancellationToken CancellationToken { get; set; }

  /// <summary>
  /// Gets or sets the request headers.
  /// </summary>
  public List<HttpHeader> Headers { get; set; }

  /// <summary>
  /// Gets or sets a value indicating whether or not to throw an <see cref="HttpFailureException"/> when a HTTP response does not indicate success.
  /// </summary>
  public bool ThrowOnFailure { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpRequestContext"/> class.
  /// </summary>
  public HttpRequestContext()
  {
    Headers = [];
    ThrowOnFailure = true;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpRequestContext"/> class.
  /// </summary>
  /// <param name="cancellationToken">The cancellation token.</param>
  public HttpRequestContext(CancellationToken cancellationToken) : this()
  {
    CancellationToken = cancellationToken;
  }
}
