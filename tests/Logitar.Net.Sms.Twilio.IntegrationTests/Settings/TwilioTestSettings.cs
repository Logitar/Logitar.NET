namespace Logitar.Net.Sms.Twilio.Settings;

internal record TwilioTestSettings
{
  public string AccountSid { get; set; }
  public string AuthenticationToken { get; set; }

  public CreateMessageSettings CreateMessage { get; set; }

  public TwilioTestSettings() : this(string.Empty, string.Empty, new CreateMessageSettings())
  {
  }

  public TwilioTestSettings(string accountSid, string authenticationToken, CreateMessageSettings createMessage)
  {
    AccountSid = accountSid;
    AuthenticationToken = authenticationToken;
    CreateMessage = createMessage;
  }
}
