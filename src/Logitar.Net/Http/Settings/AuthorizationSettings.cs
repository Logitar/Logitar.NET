namespace Logitar.Net.Http.Settings;

/// <summary>
/// Implements the authentication/authorization settings of an HTTP API client.
/// </summary>
public record AuthorizationSettings : IAuthorizationSettings
{
  /// <summary>
  /// Gets or sets the authentication scheme.
  /// </summary>
  public string Scheme { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the authentication credentials.
  /// </summary>
  public string Credentials { get; set; } = string.Empty;
}
