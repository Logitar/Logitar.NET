namespace Logitar.Net;

/// <summary>
/// Represents client credentials.
/// </summary>
public record Credentials : ICredentials
{
  /// <summary>
  /// Gets or sets the client identifier, such as an user name.
  /// </summary>
  public string Identifier { get; set; }
  /// <summary>
  /// Gets or sets the client secret, such as an user password.
  /// </summary>
  public string Secret { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="Credentials"/> class.
  /// </summary>
  public Credentials() : this(string.Empty, string.Empty)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="Credentials"/> class.
  /// </summary>
  /// <param name="identifier">The client identifier.</param>
  /// <param name="secret">The client secret.</param>
  public Credentials(string identifier, string secret)
  {
    Identifier = identifier;
    Secret = secret;
  }

  /// <summary>
  /// Parses credentials from the specified string, using the '{identifier}:{secret}' format.
  /// </summary>
  /// <param name="credentials">The credentials to parse.</param>
  /// <returns>The parsed credentials.</returns>
  public static Credentials? Parse(string? credentials)
  {
    credentials = credentials?.CleanTrim();
    if (credentials == null)
    {
      return null;
    }

    int index = credentials.IndexOf(':');
    if (index < 0)
    {
      return new Credentials(credentials, secret: string.Empty);
    }

    return new Credentials(credentials[..index], credentials[(index + 1)..]);
  }
}
