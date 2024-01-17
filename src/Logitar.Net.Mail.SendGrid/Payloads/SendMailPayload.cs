namespace Logitar.Net.Mail.SendGrid.Payloads;

/// <summary>
/// Represents the payload to send email messages.
/// The following fields are not supported yet: attachments, template_id, headers, categories, custom_args, send_at, batch_id, asm, ip_pool_name, mail_settings, tracking_settings.
/// </summary>
public record SendMailPayload
{
  /// <summary>
  /// Gets or sets the personalizations of the message.
  /// </summary>
  [JsonPropertyName("personalizations")]
  public List<PersonalizationPayload> Personalizations { get; set; } = [];

  /// <summary>
  /// Gets or sets the sender of the message.
  /// </summary>
  [JsonPropertyName("from")]
  public SenderPayload Sender { get; set; } = new();

  /// <summary>
  /// Gets or sets the reply recipient of the message.
  /// </summary>
  [JsonPropertyName("reply_to")]
  public ReplyToPayload? ReplyTo { get; set; }

  /// <summary>
  /// Gets or sets the list of reply recipients of the message.
  /// </summary>
  [JsonPropertyName("reply_to_list")]
  public List<ReplyToPayload>? ReplyToList { get; set; }

  /// <summary>
  /// Gets or sets the subject of the message.
  /// </summary>
  [JsonPropertyName("subject")]
  public string Subject { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the contents of the message.
  /// </summary>
  [JsonPropertyName("content")]
  public List<ContentPayload> Contents { get; set; } = [];

  /// <summary>
  /// Builds a send mail payload from the specified email message.
  /// </summary>
  /// <param name="message">The message.</param>
  /// <returns>The built payload.</returns>
  /// <exception cref="ArgumentException">The message did not declare a sender nor a from address.</exception>
  public static SendMailPayload FromMailMessage(MailMessage message)
  {
    MailAddress sender = message.Sender ?? message.From
      ?? throw new ArgumentException($"At least one of the following properties must be provided: {nameof(message.Sender)}, {nameof(message.From)}.", nameof(message));

    SendMailPayload payload = new()
    {
      Sender = SenderPayload.FromMailAddress(sender),
      Subject = message.Subject
    };

    payload.Personalizations.Add(PersonalizationPayload.FromMailMessage(message));

#pragma warning disable 0618
    if (message.ReplyTo != null)
    {
      // NOTE(fpion): the ReplyTo property has been marked as obsolete in MailMessage, but is still supported by SendGrid.
      payload.ReplyTo = ReplyToPayload.FromMailAddress(message.ReplyTo);
    }
#pragma warning restore 0618
    if (message.ReplyToList.Any())
    {
      payload.ReplyToList = message.ReplyToList.Select(ReplyToPayload.FromMailAddress).ToList();
    }

    payload.Contents.Add(ContentPayload.FromMailMessage(message));

    return payload;
  }
}
