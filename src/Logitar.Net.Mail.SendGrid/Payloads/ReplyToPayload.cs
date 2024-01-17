namespace Logitar.Net.Mail.SendGrid.Payloads;

/// <summary>
/// Represents a reply recipient of a message.
/// </summary>
public record ReplyToPayload
{
  /// <summary>
  /// Gets or sets the email address of the reply recipient.
  /// </summary>
  [JsonPropertyName("email")]
  public string Address { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the display name of the reply recipient.
  /// </summary>
  [JsonPropertyName("name")]
  public string? DisplayName { get; set; }

  /// <summary>
  /// Builds a reply recipient from the specified email.
  /// </summary>
  /// <param name="email">The email.</param>
  /// <returns>The built reply recipient.</returns>
  public static ReplyToPayload FromMailAddress(MailAddress email) => new()
  {
    Address = email.Address,
    DisplayName = email.DisplayName
  };
}
