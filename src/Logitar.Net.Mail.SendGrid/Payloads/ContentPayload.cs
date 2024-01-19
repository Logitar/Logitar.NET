namespace Logitar.Net.Mail.SendGrid.Payloads;

/// <summary>
/// Represents contents of an email.
/// </summary>
public record ContentPayload
{
  /// <summary>
  /// Gets or sets MIME type of the content.
  /// </summary>
  [JsonPropertyName("type")]
  public string Type { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the textual content.
  /// </summary>
  [JsonPropertyName("value")]
  public string Value { get; set; } = string.Empty;

  /// <summary>
  /// Initializes a new instance of the <see cref="ContentPayload"/> class.
  /// </summary>
  public ContentPayload()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ContentPayload"/> class.
  /// </summary>
  /// <param name="type">The MIME type of the content.</param>
  /// <param name="value">The textual content.</param>
  public ContentPayload(string type, string value)
  {
    Type = type;
    Value = value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ContentPayload"/> class.
  /// </summary>
  /// <param name="message">An email message.#</param>
  public ContentPayload(MailMessage message)
  {
    Type = message.IsBodyHtml ? MediaTypeNames.Text.Html : MediaTypeNames.Text.Plain;
    Value = message.Body;
  }
}
