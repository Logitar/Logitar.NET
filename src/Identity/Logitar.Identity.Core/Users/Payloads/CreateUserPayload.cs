namespace Logitar.Identity.Core.Users.Payloads;

public record CreateUserPayload
{
  public string? TenantId { get; set; }

  public string UniqueName { get; set; } = string.Empty;
  public string? Password { get; set; }

  public bool IsDisabled { get; set; }

  public CreateEmailPayload? Email { get; set; }

  public string? FirstName { get; set; }
  public string? LastName { get; set; }

  public string? Locale { get; set; }
}
