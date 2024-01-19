namespace Logitar.Net.Mail.SendGrid.Settings;

/// <summary>
/// Implements the settings of the SendGrid API.
/// </summary>
public record SendGridSettings : ISendGridSettings
{
  /// <summary>
  /// Gets or sets the API key used to authorize the SendGrid API calls.
  /// </summary>
  public string? ApiKey { get; set; }

  /// <summary>
  /// Gets or sets the base Uniform Resource Locator (URL) of the SendGrid API.
  /// </summary>
  public string BaseUrl { get; set; } = "https://api.sendgrid.com";
  /// <summary>
  /// Gets the base Uniform Resource Identifier (URI) of the SendGrid API.
  /// </summary>
  public Uri BaseUri => new(BaseUrl, UriKind.Absolute);

  /// <summary>
  /// Gets or sets the send mail end point information.
  /// </summary>
  public EndPointSettings SendMail { get; set; } = new()
  {
    Method = "POST",
    Path = "/v3/mail/send"
  };

  /// <summary>
  /// Initializes a new instance of the <see cref="SendGridSettings"/> class.
  /// </summary>
  public SendGridSettings() : this(apiKey: null)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="SendGridSettings"/> class.
  /// </summary>
  /// <param name="apiKey">The API key used to authorize the SendGrid API calls.</param>
  public SendGridSettings(string? apiKey)
  {
    ApiKey = apiKey;
  }
}
