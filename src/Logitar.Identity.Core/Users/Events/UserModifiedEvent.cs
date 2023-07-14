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
  /// Gets or sets the birthdate of the user.
  /// </summary>
  public Modification<DateTime?> Birthdate { get; set; }
  /// <summary>
  /// Gets or sets the gender of the user.
  /// </summary>
  public Modification<Gender> Gender { get; set; }
  /// <summary>
  /// Gets or sets the locale of the user.
  /// </summary>
  public Modification<CultureInfo> Locale { get; set; }
  /// <summary>
  /// Gets or sets the time zone of the user.
  /// </summary>
  public Modification<TimeZone> TimeZone { get; set; }

  /// <summary>
  /// Gets or sets the URL to the picture of the user.
  /// </summary>
  public Modification<Uri> Picture { get; set; }
  /// <summary>
  /// Gets or sets the URL to the profile of the user.
  /// </summary>
  public Modification<Uri> Profile { get; set; }
  /// <summary>
  /// Gets or sets the URL to the website of the user.
  /// </summary>
  public Modification<Uri> Website { get; set; }

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
