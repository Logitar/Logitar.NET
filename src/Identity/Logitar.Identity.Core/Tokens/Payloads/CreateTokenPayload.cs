namespace Logitar.Identity.Core.Tokens.Payloads;

public record CreateTokenPayload
{
  public bool IsConsumable { get; set; }
  public int? Lifetime { get; set; }
  public string? Purpose { get; set; }

  public string Secret { get; set; } = string.Empty;
  public string? Algorithm { get; set; }
  public string? Audience { get; set; }
  public string? Issuer { get; set; }

  public string? Subject { get; set; }
  public string? EmailAddress { get; set; }

  public IEnumerable<TokenClaim> Claims { get; set; } = Enumerable.Empty<TokenClaim>();
}
