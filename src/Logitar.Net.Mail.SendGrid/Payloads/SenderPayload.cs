namespace Logitar.Net.Mail.SendGrid.Payloads;

/// <summary>
/// Represents the sender of a message.
/// </summary>
public record SenderPayload
{
  /// <summary>
  /// Gets or sets the email address of the sender.
  /// </summary>
  [JsonPropertyName("email")]
  public string Address { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the display name of the sender.
  /// </summary>
  [JsonPropertyName("name")]
  public string? DisplayName { get; set; }

  /// <summary>
  /// Builds a sender from the specified email.
  /// </summary>
  /// <param name="email">The email.</param>
  /// <returns>The built sender.</returns>
  public static SenderPayload FromMailAddress(MailAddress email) => new()
  {
    Address = email.Address,
    DisplayName = email.DisplayName
  };
}
