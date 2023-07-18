using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Roles.Models;

namespace Logitar.Identity.Core.Users.Models;

/// <summary>
/// Represents the read model for users.
/// </summary>
public record User : Aggregate
{
  /// <summary>
  /// Gets or sets the identifier of the tenant in which the user belongs.
  /// </summary>
  public string? TenantId { get; set; }

  /// <summary>
  /// Gets or sets the unique name of the user.
  /// </summary>
  public string UniqueName { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets a value indicating whether or not the user has a password.
  /// </summary>
  public bool HasPassword { get; set; }
  /// <summary>
  /// Gets or sets the actor who changed the user's password lastly.
  /// </summary>
  public Actor? PasswordChangedBy { get; set; }
  /// <summary>
  /// Gets or sets the latest date and time when the user password has been changed.
  /// </summary>
  public DateTime? PasswordChangedOn { get; set; }

  /// <summary>
  /// Gets or sets the actor who disabled the user.
  /// </summary>
  public Actor? DisabledBy { get; set; }
  /// <summary>
  /// Gets or sets the date and time when the user has been disabled.
  /// </summary>
  public DateTime? DisabledOn { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not the user is disabled.
  /// </summary>
  public bool IsDisabled { get; set; }

  /// <summary>
  /// Gets or sets the postal address of the user.
  /// </summary>
  public Address? Address { get; set; }
  /// <summary>
  /// Gets or sets the email address of the user.
  /// </summary>
  public Email? Email { get; set; }
  /// <summary>
  /// Gets or sets the phone number of the user.
  /// </summary>
  public Phone? Phone { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not the user is confirmed.
  /// <br />A confirmed user must have at least one verified contact information (address, email or phone).
  /// </summary>
  public bool IsConfirmed { get; set; }

  /// <summary>
  /// Gets or sets the first name of the user.
  /// </summary>
  public string? FirstName { get; set; }
  /// <summary>
  /// Gets or sets the middle name of the user.
  /// </summary>
  public string? MiddleName { get; set; }
  /// <summary>
  /// Gets or sets the last name of the user.
  /// </summary>
  public string? LastName { get; set; }
  /// <summary>
  /// Gets or sets the full name of the user.
  /// </summary>
  public string? FullName { get; private set; }
  /// <summary>
  /// Gets or sets the nickname of the user.
  /// </summary>
  public string? Nickname { get; set; }

  /// <summary>
  /// Gets or sets the birthdate of the user.
  /// </summary>
  public DateTime? Birthdate { get; set; }
  /// <summary>
  /// Gets or sets the gender of the user.
  /// </summary>
  public string? Gender { get; set; }
  /// <summary>
  /// Gets or sets the locale of the user.
  /// </summary>
  public string? Locale { get; set; }
  /// <summary>
  /// Gets or sets the time zone of the user.
  /// </summary>
  public string? TimeZone { get; set; }

  /// <summary>
  /// Gets or sets the URL to the picture of the user.
  /// </summary>
  public string? Picture { get; set; }
  /// <summary>
  /// Gets or sets the URL to the profile of the user.
  /// </summary>
  public string? Profile { get; set; }
  /// <summary>
  /// Gets or sets the URL to the website of the user.
  /// </summary>
  public string? Website { get; set; }

  /// <summary>
  /// Gets or sets the custom attributes of the user.
  /// </summary>
  public IEnumerable<CustomAttribute> CustomAttributes { get; set; } = Enumerable.Empty<CustomAttribute>();

  /// <summary>
  /// Gets or sets the external identifiers of the user.
  /// </summary>
  public IEnumerable<ExternalIdentifier> ExternalIdentifiers { get; set; } = Enumerable.Empty<ExternalIdentifier>();

  /// <summary>
  /// Gets or sets the roles of the user.
  /// </summary>
  public IEnumerable<Role> Roles { get; set; } = Enumerable.Empty<Role>();
}
