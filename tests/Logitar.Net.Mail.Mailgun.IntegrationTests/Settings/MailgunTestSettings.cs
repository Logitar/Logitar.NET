namespace Logitar.Net.Mail.Mailgun.Settings;

internal record MailgunTestSettings
{
  public string ApiKey { get; set; }
  public string DomainName { get; set; }

  public EmailSettings From { get; set; }

  public EmailSettings To { get; set; }
  public List<EmailSettings> CC { get; set; }
  public List<EmailSettings> Bcc { get; set; }

  public MailgunTestSettings() : this(string.Empty, string.Empty, new EmailSettings(), new EmailSettings())
  {
  }

  public MailgunTestSettings(string apiKey, string domainName, EmailSettings from, EmailSettings to)
  {
    ApiKey = apiKey;
    DomainName = domainName;

    From = from;

    To = to;
    CC = [];
    Bcc = [];
  }
}
