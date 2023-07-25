namespace Logitar.Identity.Core.Models;

public record SearchResults<T>
{
  public SearchResults() : this(Enumerable.Empty<T>())
  {
  }
  public SearchResults(IEnumerable<T> items) : this(items, items.LongCount())
  {
  }
  public SearchResults(long total) : this(Enumerable.Empty<T>(), total)
  {
  }
  public SearchResults(IEnumerable<T> items, long total)
  {
    Items = items;
    Total = total;
  }

  public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
  public long Total { get; set; }
}
