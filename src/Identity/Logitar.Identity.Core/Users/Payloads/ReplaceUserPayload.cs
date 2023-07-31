using Logitar.Identity.Core.Models;

namespace Logitar.Identity.Core.Users.Payloads;

public record ReplaceUserPayload
{
  public string? UniqueName { get; set; }
  public string? Password { get; set; }

  public bool? IsDisabled { get; set; }

  public UpdateAddressPayload? Address { get; set; }
  public UpdateEmailPayload? Email { get; set; }
  public UpdatePhonePayload? Phone { get; set; }

  public string? FirstName { get; set; }
  public string? MiddleName { get; set; }
  public string? LastName { get; set; }
  public string? Nickname { get; set; }

  public DateTime? Birthdate { get; set; }
  public string? Gender { get; set; }
  public string? Locale { get; set; }
  public string? TimeZone { get; set; }

  public string? Picture { get; set; }
  public string? Profile { get; set; }
  public string? Website { get; set; }

  public IEnumerable<CustomAttribute> CustomAttributes { get; set; } = Enumerable.Empty<CustomAttribute>();

  public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
}
