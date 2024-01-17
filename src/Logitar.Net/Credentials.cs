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
}
