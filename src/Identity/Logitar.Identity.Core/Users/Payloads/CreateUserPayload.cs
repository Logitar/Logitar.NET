using Logitar.Identity.Core.Models;

namespace Logitar.Identity.Core.Users.Payloads;

public record CreateUserPayload
{
  public string? TenantId { get; set; }

  public string UniqueName { get; set; } = string.Empty;
  public string? Password { get; set; }

  public bool IsDisabled { get; set; }

  public CreateAddressPayload? Address { get; set; }
  public CreateEmailPayload? Email { get; set; }
  public CreatePhonePayload? Phone { get; set; }

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
