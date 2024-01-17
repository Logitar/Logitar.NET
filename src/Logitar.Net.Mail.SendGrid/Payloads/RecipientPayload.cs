namespace Logitar.Net.Mail.SendGrid.Payloads;

/// <summary>
/// Represents a recipient of a message.
/// </summary>
public record RecipientPayload
{
  /// <summary>
  /// Gets or sets the email address of the recipient.
  /// </summary>
  [JsonPropertyName("email")]
  public string Address { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the display name of the recipient.
  /// </summary>
  [JsonPropertyName("name")]
  public string? DisplayName { get; set; }

  /// <summary>
  /// Builds a recipient from the specified email.
  /// </summary>
  /// <param name="email">The email.</param>
  /// <returns>The built recipient.</returns>
  public static RecipientPayload FromMailAddress(MailAddress email) => new()
  {
    Address = email.Address,
    DisplayName = email.DisplayName
  };
}
