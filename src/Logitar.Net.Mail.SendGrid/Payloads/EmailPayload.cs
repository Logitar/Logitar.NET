namespace Logitar.Net.Mail.SendGrid.Payloads;

/// <summary>
/// Represents an email component.
/// </summary>
public record EmailPayload
{
  /// <summary>
  /// Gets or sets the email address.
  /// </summary>
  [JsonPropertyName("email")]
  public string Address { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the display name associated to this email.
  /// </summary>
  [JsonPropertyName("name")]
  public string? DisplayName { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="EmailPayload"/> class.
  /// </summary>
  public EmailPayload()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="EmailPayload"/> class.
  /// </summary>
  /// <param name="address">The email address.</param>
  /// <param name="displayName">The display name associated to this email.</param>
  public EmailPayload(string address, string? displayName = null)
  {
    Address = address;
    DisplayName = displayName;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="EmailPayload"/> class.
  /// </summary>
  /// <param name="email">An email address.</param>
  public EmailPayload(MailAddress email) : this(email.Address, email.DisplayName)
  {
  }
}
