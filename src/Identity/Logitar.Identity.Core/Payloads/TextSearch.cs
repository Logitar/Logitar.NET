namespace Logitar.Identity.Core.Payloads;

public record TextSearch
{
  public IEnumerable<SearchTerm> Terms { get; set; } = Enumerable.Empty<SearchTerm>();
  public SearchOperator Operator { get; set; }
}
