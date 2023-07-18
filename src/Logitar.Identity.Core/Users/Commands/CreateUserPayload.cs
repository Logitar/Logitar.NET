using Logitar.Identity.Core.Models;

namespace Logitar.Identity.Core.Users.Commands;

/// <summary>
/// The user creation data.
/// </summary>
public record CreateUserPayload
{
  /// <summary>
  /// Gets or sets the identifier of the user.
  /// </summary>
  public string? Id { get; set; }

  /// <summary>
  /// Gets or sets the identifier of the tenant in which the user belongs.
  /// </summary>
  public string? TenantId { get; set; }

  /// <summary>
  /// Gets or sets the unique name of the user.
  /// </summary>
  public string UniqueName { get; set; } = string.Empty;
  /// <summary>
  /// Gets or sets the password of the user.
  /// </summary>
  public string? Password { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not the user is disabled.
  /// </summary>
  public bool IsDisabled { get; set; }

  /// <summary>
  /// Gets or sets the postal address of the user.
  /// </summary>
  public AddressPayload? Address { get; set; }
  /// <summary>
  /// Gets or sets the email address of the user.
  /// </summary>
  public EmailPayload? Email { get; set; }
  /// <summary>
  /// Gets or sets the phone number of the user.
  /// </summary>
  public PhonePayload? Phone { get; set; }

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
  public IEnumerable<CustomAttribute>? CustomAttributes { get; set; }

  /// <summary>
  /// Gets or sets the roles of the user.
  /// <br />Each item in the list can either be the identifier or the unique name of a role.
  /// <br />Each specified role must reside into the same tenant as the user.
  /// </summary>
  public IEnumerable<string>? Roles { get; set; }
}
