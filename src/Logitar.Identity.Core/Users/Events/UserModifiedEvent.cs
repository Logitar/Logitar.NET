using Logitar.EventSourcing;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when an <see cref="UserAggregate"/> is modified.
/// </summary>
public record UserModifiedEvent : DomainEvent
{
  /// <summary>
  /// Gets or sets the first name of the user.
  /// </summary>
  public Modification<string> FirstName { get; set; }
  /// <summary>
  /// Gets or sets the middle name of the user.
  /// </summary>
  public Modification<string> MiddleName { get; set; }
  /// <summary>
  /// Gets or sets the last name of the user.
  /// </summary>
  public Modification<string> LastName { get; set; }
  /// <summary>
  /// Gets or sets the full name of the user.
  /// </summary>
  public Modification<string> FullName { get; set; }
  /// <summary>
  /// Gets or sets the nickname of the user.
  /// </summary>
  public Modification<string> Nickname { get; set; }

  /// <summary>
  /// Gets or sets the custom attribute modifications of the user.
  /// <br />If the value is null, the custom attribute will be removed.
  /// <br />Otherwise, the custom attribute will be added or replaced.
  /// </summary>
  public Dictionary<string, string?> CustomAttributes { get; init; } = new();
  /// <summary>
  /// Gets or sets the role modifications of the user.
  /// <br />The key of the dictionary corresponds to the identifier of the role.
  /// <br />If the value is true, then the role will be added.
  /// <br />Otherwise, the role will be removed.
  /// </summary>
  public Dictionary<string, bool> Roles { get; init; } = new();
}
