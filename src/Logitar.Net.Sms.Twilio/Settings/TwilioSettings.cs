namespace Logitar.Net.Sms.Twilio.Settings;

/// <summary>
/// Implements the settings of the Twilio API.
/// </summary>
public record TwilioSettings : ITwilioSettings
{
  /// <summary>
  /// Gets or sets the Twilio account Security IDentifier.
  /// </summary>
  public string? AccountSid { get; set; }
  /// <summary>
  /// Gets or sets the Twilio account authentication token.
  /// </summary>
  public string? AuthenticationToken { get; set; }

  /// <summary>
  /// Gets or sets the base Uniform Resource Locator (URL) of the Twilio API.
  /// </summary>
  public string BaseUrl { get; set; } = "https://api.twilio.com";
  /// <summary>
  /// Gets the base Uniform Resource Identifier (URI) of the Twilio API.
  /// </summary>
  public Uri BaseUri => new(BaseUrl, UriKind.Absolute);

  /// <summary>
  /// Gets or sets the create message end point information.
  /// </summary>
  public EndPointSettings CreateMessage { get; set; } = new()
  {
    Method = "POST",
    Path = "/2010-04-01/Accounts/{AccountSid}/Messages.json"
  };

  /// <summary>
  /// Initializes a new instance of the <see cref="TwilioSettings"/> class.
  /// </summary>
  public TwilioSettings() : this(accountSid: null, authenticationToken: null)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="TwilioSettings"/> class.
  /// </summary>
  /// <param name="accountSid">The Twilio account Security IDentifier.</param>
  /// <param name="authenticationToken">The Twilio account authentication token.</param>
  public TwilioSettings(string? accountSid, string? authenticationToken)
  {
    AccountSid = accountSid;
    AuthenticationToken = authenticationToken;
  }
}
