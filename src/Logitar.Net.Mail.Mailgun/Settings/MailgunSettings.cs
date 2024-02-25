namespace Logitar.Net.Mail.Mailgun.Settings;

/// <summary>
/// Implements the settings of the Mailgun API.
/// </summary>
public record MailgunSettings : IMailgunSettings
{
  /// <summary>
  /// Gets the username used to authorize the Mailgun API calls.
  /// </summary>
  public string Username { get; set; } = "api";
  /// <summary>
  /// Gets the password used to authorize the Mailgun API calls, typically your API key.
  /// </summary>
  public string? Password => ApiKey;
  /// <summary>
  /// Gets or sets the API key used to authorize the Mailgun API calls.
  /// </summary>
  public string? ApiKey { get; set; }

  /// <summary>
  /// Gets or sets the base Uniform Resource Locator (URL) of the Mailgun API.
  /// </summary>
  public string BaseUrl { get; set; } = "https://api.mailgun.net";
  /// <summary>
  /// Gets the base Uniform Resource Identifier (URI) of the Mailgun API.
  /// </summary>
  public Uri BaseUri => new(BaseUrl, UriKind.Absolute);

  /// <summary>
  /// Gets the name of the domain used to send messages from.
  /// </summary>
  public string? DomainName { get; set; }

  /// <summary>
  /// Gets or sets the send mail end point information.
  /// </summary>
  public EndPointSettings SendMail { get; set; } = new()
  {
    Method = "POST",
    Path = "/v3/{DomainName}/messages"
  };

  /// <summary>
  /// Initializes a new instance of the <see cref="MailgunSettings"/> class.
  /// </summary>
  public MailgunSettings() : this(apiKey: null, domainName: null)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="MailgunSettings"/> class.
  /// </summary>
  /// <param name="apiKey">The API key used to authorize the Mailgun API calls.</param>
  /// <param name="domainName">The name of the domain used to send messages from.</param>
  public MailgunSettings(string? apiKey, string? domainName)
  {
    ApiKey = apiKey;
    DomainName = domainName;
  }
}
