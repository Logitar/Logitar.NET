namespace Logitar.Identity.Core.Users.Payloads;

public record AuthenticateUserPayload
{
  public string? TenantId { get; set; }
  public string UniqueName { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
}
