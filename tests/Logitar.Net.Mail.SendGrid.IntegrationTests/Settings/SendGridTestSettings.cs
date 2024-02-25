namespace Logitar.Net.Mail.SendGrid.Settings;

internal record SendGridTestSettings
{
  public string ApiKey { get; set; }

  public EmailSettings From { get; set; }

  public EmailSettings To { get; set; }
  public List<EmailSettings> CC { get; set; }
  public List<EmailSettings> Bcc { get; set; }

  public SendGridTestSettings() : this(string.Empty, new EmailSettings(), new EmailSettings())
  {
  }

  public SendGridTestSettings(string apiKey, EmailSettings from, EmailSettings to)
  {
    ApiKey = apiKey;

    From = from;

    To = to;
    CC = [];
    Bcc = [];
  }
}
