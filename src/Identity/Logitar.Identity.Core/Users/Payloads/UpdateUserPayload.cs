using Logitar.Identity.Domain;

namespace Logitar.Identity.Core.Users.Payloads;

public record UpdateUserPayload
{
  public string? UniqueName { get; set; }
  public string? Password { get; set; }

  public bool? IsDisabled { get; set; }

  public MayBe<UpdateEmailPayload>? Email { get; set; }
  public MayBe<UpdatePhonePayload>? Phone { get; set; }

  public MayBe<string>? FirstName { get; set; }
  public MayBe<string>? MiddleName { get; set; }
  public MayBe<string>? LastName { get; set; }
  public MayBe<string>? Nickname { get; set; }

  public MayBe<DateTime?>? Birthdate { get; set; }
  public MayBe<string>? Gender { get; set; }
  public MayBe<string>? Locale { get; set; }
  public MayBe<string>? TimeZone { get; set; }

  public MayBe<string>? Picture { get; set; }
  public MayBe<string>? Profile { get; set; }
  public MayBe<string>? Website { get; set; }
}
