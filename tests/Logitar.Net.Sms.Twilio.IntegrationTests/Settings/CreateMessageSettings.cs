namespace Logitar.Net.Sms.Twilio.Settings;

internal record CreateMessageSettings
{
  public string From { get; set; }
  public string To { get; set; }
  public string Body { get; set; }

  public CreateMessageSettings() : this(string.Empty, string.Empty, string.Empty)
  {
  }

  public CreateMessageSettings(string from, string to, string body)
  {
    From = from;
    To = to;
    Body = body;
  }
}
