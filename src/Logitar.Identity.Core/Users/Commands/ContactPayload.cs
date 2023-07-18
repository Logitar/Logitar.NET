namespace Logitar.Identity.Core.Users.Commands;

/// <summary>
/// The base contact information input data.
/// </summary>
public abstract record ContactPayload
{
  /// <summary>
  /// Gets or sets a value indicating whether or not the contact is verified.
  /// <br />If null, the contact verification status will not change.
  /// </summary>
  public bool? IsVerified { get; set; }
}
