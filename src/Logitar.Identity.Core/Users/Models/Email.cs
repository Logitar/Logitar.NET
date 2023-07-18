namespace Logitar.Identity.Core.Users.Models;

/// <summary>
/// Represents the read model for email addresses.
/// </summary>
public record Email : Contact
{
  /// <summary>
  /// Gets or sets the email address.
  /// </summary>
  public string Address { get; set; } = string.Empty;
}
