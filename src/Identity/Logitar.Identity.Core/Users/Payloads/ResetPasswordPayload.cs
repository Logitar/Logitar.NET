namespace Logitar.Identity.Core.Users.Payloads;

public record ResetPasswordPayload
{
  public string Secret { get; set; } = string.Empty;
  public string? Audience { get; set; }
  public string? Issuer { get; set; }

  public string Token { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
}
