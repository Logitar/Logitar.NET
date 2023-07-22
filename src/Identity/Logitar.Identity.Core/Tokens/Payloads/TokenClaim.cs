namespace Logitar.Identity.Core.Tokens.Payloads;

public record TokenClaim
{
  public string Type { get; set; } = string.Empty;
  public string Value { get; set; } = string.Empty;
  public string? ValueType { get; set; }
}
