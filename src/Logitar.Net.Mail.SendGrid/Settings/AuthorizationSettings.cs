namespace Logitar.Net.Mail.SendGrid.Settings;

/// <summary>
/// Represents the authentication/authorization settings of the SendGrid API.
/// </summary>
public record AuthorizationSettings : IAuthorizationSettings
{
  /// <summary>
  /// Gets or sets the SendGrid API key.
  /// </summary>
  public string? ApiKey { get; set; }
  /// <summary>
  /// Gets or sets the authentication scheme.
  /// </summary>
  public string Scheme { get; set; } = "Bearer";
}
