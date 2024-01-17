namespace Logitar.Net.Mail.SendGrid.Payloads;

/// <summary>
/// Represents a message personalization, information that is sent to a subset of recipients.
/// The following fields are not supported yet: headers, substitutions, dynamic_template_data, custom_args, send_at.
/// </summary>
public record PersonalizationPayload
{
  /// <summary>
  /// Gets or sets the sender of the personalization.
  /// </summary>
  [JsonPropertyName("from")]
  public SenderPayload? Sender { get; set; }

  /// <summary>
  /// Gets or sets the To recipients of the personalization.
  /// </summary>
  [JsonPropertyName("to")]
  public List<RecipientPayload> To { get; set; } = [];

  /// <summary>
  /// Gets or sets the CC recipients of the personalization.
  /// </summary>
  [JsonPropertyName("cc")]
  public List<RecipientPayload>? CC { get; set; }

  /// <summary>
  /// Gets or sets the Bcc recipients of the personalization.
  /// </summary>
  [JsonPropertyName("bcc")]
  public List<RecipientPayload>? Bcc { get; set; }

  /// <summary>
  /// Gets or sets the subject of the personalization.
  /// </summary>
  [JsonPropertyName("subject")]
  public string? Subject { get; set; }

  /// <summary>
  /// Builds an email personalization from the specified message.
  /// </summary>
  /// <param name="message">The message.</param>
  /// <returns>The built personalization.</returns>
  public static PersonalizationPayload FromMailMessage(MailMessage message)
  {
    PersonalizationPayload payload = new();

    foreach (MailAddress to in message.To)
    {
      payload.To.Add(RecipientPayload.FromMailAddress(to));
    }
    if (message.CC.Any())
    {
      payload.CC = message.CC.Select(RecipientPayload.FromMailAddress).ToList();
    }
    foreach (MailAddress bcc in message.Bcc)
    {
      payload.Bcc = message.Bcc.Select(RecipientPayload.FromMailAddress).ToList();
    }

    return payload;
  }
}
