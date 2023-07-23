using Logitar.Identity.Core.Tokens.Payloads;

namespace Logitar.Identity.Core.Tokens.Models;
public record ValidatedToken
{
  public string? Subject { get; set; }
  public string? EmailAddress { get; set; }
  public IEnumerable<TokenClaim> Claims { get; set; } = Enumerable.Empty<TokenClaim>();
}
