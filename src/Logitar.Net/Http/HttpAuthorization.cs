namespace Logitar.Net.Http;

/// <summary>
/// Represents HTTP authorization parameters.
/// </summary>
public record HttpAuthorization : IHttpAuthorization
{
  /// <summary>
  /// Gets or sets the authentication scheme.
  /// </summary>
  public string Scheme { get; set; }
  /// <summary>
  /// Gets or sets the client credentials.
  /// </summary>
  public string Credentials { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpAuthorization"/> class.
  /// </summary>
  public HttpAuthorization() : this(string.Empty, string.Empty)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpAuthorization"/> class.
  /// </summary>
  /// <param name="scheme">The authentication scheme.</param>
  /// <param name="credentials">The client credentials.</param>
  public HttpAuthorization(string scheme, string credentials)
  {
    Scheme = scheme;
    Credentials = credentials;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpAuthorization"/> class using the BASIC scheme.
  /// </summary>
  /// <param name="identifier">The client identifier, such as an user name.</param>
  /// <param name="secret">The client secret, such as an user password.</param>
  /// <returns>The authorization parameters.</returns>
  public static HttpAuthorization Basic(string identifier, string secret) => Basic(new Credentials(identifier, secret));
  /// <summary>
  /// Initializes a new instance of the <see cref="HttpAuthorization"/> class using the BASIC scheme.
  /// </summary>
  /// <param name="credentials">The client credentials.</param>
  /// <returns>The authorization parameters.</returns>
  public static HttpAuthorization Basic(ICredentials credentials)
  {
    string formatted = $"{credentials.Identifier}:{credentials.Secret}";
    byte[] bytes = Encoding.UTF8.GetBytes(formatted);
    string base64 = Convert.ToBase64String(bytes);

    return Basic(base64);
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="HttpAuthorization"/> class using the BASIC scheme.
  /// </summary>
  /// <param name="credentials">The base64-encoded credentials, using the '{identifier}:{secret}' format.</param>
  /// <returns>The authorization parameters.</returns>
  public static HttpAuthorization Basic(string credentials) => new(AuthenticationSchemes.Basic, credentials);
  /// <summary>
  /// Initializes a new instance of the <see cref="HttpAuthorization"/> class using the BEARER scheme.
  /// </summary>
  /// <param name="credentials">The client credentials, such as an API key or a Bearer/Access token.</param>
  /// <returns>The authorization parameters.</returns>
  public static HttpAuthorization Bearer(string credentials) => new(AuthenticationSchemes.Bearer, credentials);
}
