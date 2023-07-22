namespace Logitar.Identity.Core.Tokens.Payloads;

public record TokenClaim
{
  public TokenClaim() : this(string.Empty, string.Empty)
  {
  }
  public TokenClaim(string type, string value, string? valueType = null)
  {
    Type = type;
    Value = value;
    ValueType = valueType;
  }

  public string Type { get; set; }
  public string Value { get; set; }
  public string? ValueType { get; set; }
}
