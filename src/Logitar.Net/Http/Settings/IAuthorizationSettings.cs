namespace Logitar.Net.Http.Settings;

/// <summary>
/// Defines the authentication/authorization settings of an HTTP API client.
/// </summary>
public interface IAuthorizationSettings
{
  /// <summary>
  /// Gets the authentication scheme.
  /// </summary>
  string Scheme { get; }

  /// <summary>
  /// Gets the authentication credentials.
  /// </summary>
  string Credentials { get; }
}
