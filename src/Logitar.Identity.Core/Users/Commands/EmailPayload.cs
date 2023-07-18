namespace Logitar.Identity.Core.Users.Commands;

/// <summary>
/// The email address input data.
/// </summary>
public record EmailPayload : ContactPayload
{
  /// <summary>
  /// Gets or sets the email address.
  /// </summary>
  public string Address { get; set; } = string.Empty;
}
