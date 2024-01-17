namespace Logitar.Net.Http;

/// <summary>
/// Defines HTTP authorization parameters.
/// </summary>
public interface IHttpAuthorization
{
  /// <summary>
  /// Gets the authentication scheme.
  /// </summary>
  string Scheme { get; }
  /// <summary>
  /// Gets the client credentials.
  /// </summary>
  string Credentials { get; }
}
