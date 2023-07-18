namespace Logitar.Identity.Core.Models;

/// <summary>
/// Represents an actor in the identity system. It can be an API key, an user, or a user-defined type.
/// </summary>
public record Actor
{
  /// <summary>
  /// Gets or sets the identifier of the actor. Defaults to SYSTEM.
  /// </summary>
  public string Id { get; set; } = "SYSTEM";
  /// <summary>
  /// Gets or sets the type of the actor. Defaults to System.
  /// </summary>
  public string Type { get; set; } = "System";
  /// <summary>
  /// Gets or sets a value indicating whether or not the actor is deleted. Defaults to false.
  /// </summary>
  public bool IsDeleted { get; set; }

  /// <summary>
  /// Gets or sets the display name of the actor. Defaults to System.
  /// </summary>
  public string DisplayName { get; set; } = "System";
  /// <summary>
  /// Gets or sets the email address of the actor, if any.
  /// </summary>
  public string? EmailAddress { get; set; }
  /// <summary>
  /// Gets or sets the URL to the picture of the actor, if any.
  /// </summary>
  public string? PictureUrl { get; set; }
}
