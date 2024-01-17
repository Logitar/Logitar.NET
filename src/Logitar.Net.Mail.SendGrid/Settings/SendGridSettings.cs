namespace Logitar.Net.Mail.SendGrid.Settings;

/// <summary>
/// Represents the settings required to send emails from the SendGrid API.
/// </summary>
public record SendGridSettings : ISendGridSettings
{
  /// <summary>
  /// Gets or sets the authentication/authorization settings of the SendGrid API.
  /// </summary>
  public IAuthorizationSettings Authorization { get; set; } = new AuthorizationSettings();

  /// <summary>
  /// Gets or sets the base Uniform Resource Locator (URL) of the SendGrid API.
  /// </summary>
  public string BaseUrl { get; set; } = "https://api.sendgrid.com";
  /// <summary>
  /// Gets the base Uniform Resource Identifier (URI) of the SendGrid API.
  /// </summary>
  public Uri BaseUri => new(BaseUrl, UriKind.Absolute);

  /// <summary>
  /// Gets or sets the send mail endpoint of the SendGrid API.
  /// </summary>
  public IEndPointSettings SendMail { get; set; } = new EndPointSettings()
  {
    Method = "POST",
    Path = "/v3/mail/send"
  };
}
