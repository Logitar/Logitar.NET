namespace Logitar.Net.Mail.SendGrid.Payloads;

/// <summary>
/// Represents the data required to send an email message.
/// </summary>
public record SendMailPayload
{
  /// <summary>
  /// Gets or sets a list of personalized message envelopes.
  /// </summary>
  [JsonPropertyName("personalizations")]
  public List<PersonalizationPayload> Personalizations { get; set; } = [];

  /// <summary>
  /// Gets or sets the sender email of the message.
  /// </summary>
  [JsonPropertyName("from")]
  public EmailPayload From { get; set; } = new();

  /// <summary>
  /// Gets or sets the recipient who will receive replies.
  /// </summary>
  [JsonPropertyName("reply_to")]
  public EmailPayload? ReplyTo { get; set; }

  /// <summary>
  /// Gets or sets a list of recipients who will receive replies.
  /// </summary>
  [JsonPropertyName("reply_to_list")]
  public List<EmailPayload>? ReplyToList { get; set; }

  /// <summary>
  /// Gets or sets the subject of the message.
  /// </summary>
  [JsonPropertyName("subject")]
  public string Subject { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the message contents.
  /// </summary>
  [JsonPropertyName("content")]
  public List<ContentPayload> Contents { get; set; } = [];

  /// <summary>
  /// Initializes a new instance of the <see cref="SendMailPayload"/> class.
  /// </summary>
  public SendMailPayload()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="SendMailPayload"/> class.
  /// </summary>
  /// <param name="message">An email message.</param>
  /// <exception cref="ArgumentException">The email message did not provide a sender nor a from email.</exception>
  public SendMailPayload(MailMessage message)
  {
    MailAddress from = message.Sender ?? message.From
      ?? throw new ArgumentException($"At least one of the following must be provided: {message.From}, {nameof(message.Sender)}.", nameof(message));

    Personalizations.Add(new(message));
    From = new(from);
    Subject = message.Subject;
    Contents.Add(new(message));

#pragma warning disable 0618
    // NOTE(fpion): the MailMessage.ReplyTo property is marked as obsolete, but it is still supported by SendGrid.
    if (message.ReplyTo != null)
    {
      ReplyTo = new(message.ReplyTo);
    }
#pragma warning restore 0618
    if (message.ReplyToList.Any())
    {
      ReplyToList = message.ReplyToList.Select(recipient => new EmailPayload(recipient)).ToList();
    }
  }
}
