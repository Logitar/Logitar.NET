using Logitar.Identity.Domain;

namespace Logitar.Identity.Core.Users.Payloads;

public record UpdateUserPayload
{
  public string? UniqueName { get; set; }
  public string? Password { get; set; }

  public bool? IsDisabled { get; set; }

  public MayBe<UpdateEmailPayload>? Email { get; set; }

  public MayBe<string>? FirstName { get; set; }
  public MayBe<string>? LastName { get; set; }

  public MayBe<string>? Locale { get; set; }
}
