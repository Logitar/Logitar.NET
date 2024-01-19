namespace Logitar.Net.Mail.SendGrid.Payloads;

/// <summary>
/// Represents a personalized message envelope.
/// </summary>
public record PersonalizationPayload
{
  /// <summary>
  /// Gets or sets the sender email.
  /// </summary>
  [JsonPropertyName("from")]
  public EmailPayload? From { get; set; }

  /// <summary>
  /// Gets or sets a list of intended recipients.
  /// </summary>
  [JsonPropertyName("to")]
  public List<EmailPayload> To { get; set; } = [];

  /// <summary>
  /// Gets or sets a list of recipients who will receive a carbon copy of the message.
  /// </summary>
  [JsonPropertyName("cc")]
  public List<EmailPayload>? CC { get; set; }

  /// <summary>
  /// Gets or sets a list of recipients who will receive a blind carbon copy of the message.
  /// </summary>
  [JsonPropertyName("bcc")]
  public List<EmailPayload>? Bcc { get; set; }

  /// <summary>
  /// Gets or sets the personalized message subject.
  /// </summary>
  [JsonPropertyName("subject")]
  public string? Subject { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="PersonalizationPayload"/> class.
  /// </summary>
  public PersonalizationPayload()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="PersonalizationPayload"/> class.
  /// </summary>
  /// <param name="message">An email message.</param>
  public PersonalizationPayload(MailMessage message)
  {
    To.AddRange(message.To.Select(recipient => new EmailPayload(recipient)));

    if (message.CC.Any())
    {
      CC = message.CC.Select(recipient => new EmailPayload(recipient)).ToList();
    }

    if (message.Bcc.Any())
    {
      Bcc = message.Bcc.Select(recipient => new EmailPayload(recipient)).ToList();
    }
  }
}
