namespace Logitar.Net.Mail.SendGrid.Settings;

/// <summary>
/// The authentication/authorization settings of the SendGrid API.
/// </summary>
public interface IAuthorizationSettings
{
  /// <summary>
  /// Gets or sets the SendGrid API key.
  /// </summary>
  string? ApiKey { get; }
  /// <summary>
  /// Gets or sets the authentication scheme.
  /// </summary>
  string Scheme { get; }
}
