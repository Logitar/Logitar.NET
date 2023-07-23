namespace Logitar.Identity.Core.Tokens.Payloads;

public record ValidateTokenPayload
{
  public string Token { get; set; } = string.Empty;

  public string Secret { get; set; } = string.Empty;
  public string? Audience { get; set; }
  public string? Issuer { get; set; }
  public string? Purpose { get; set; }
}
