namespace Logitar.Identity.Core.Users.Payloads;

public record RecoverPasswordPayload
{
  public string Secret { get; set; } = string.Empty;
  public string? Algorithm { get; set; }
  public string? Audience { get; set; }
  public string? Issuer { get; set; }

  public string? TenantId { get; set; }
  public string UniqueName { get; set; } = string.Empty;
}
