namespace Logitar.Net.Mail.SendGrid.Payloads;

/// <summary>
/// Represents the content of an email message.
/// </summary>
public record ContentPayload
{
  /// <summary>
  /// Gets or sets the MIME type of the content.
  /// </summary>
  [JsonPropertyName("type")]
  public string Type { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the value of the content.
  /// </summary>
  [JsonPropertyName("value")]
  public string Value { get; set; } = string.Empty;

  /// <summary>
  /// Builds an email content from the specified message.
  /// </summary>
  /// <param name="message">The message.</param>
  /// <returns>The built content.</returns>
  public static ContentPayload FromMailMessage(MailMessage message) => new()
  {
    Type = message.IsBodyHtml ? MediaTypeNames.Text.Html : MediaTypeNames.Text.Plain,
    Value = message.Body
  };
}
