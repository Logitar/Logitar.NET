namespace Logitar.Net;

/// <summary>
/// Defines client credentials.
/// </summary>
public interface ICredentials
{
  /// <summary>
  /// Gets the client identifier, such as an user name.
  /// </summary>
  string Identifier { get; }
  /// <summary>
  /// Gets the client secret, such as an user password.
  /// </summary>
  string Secret { get; }
}
