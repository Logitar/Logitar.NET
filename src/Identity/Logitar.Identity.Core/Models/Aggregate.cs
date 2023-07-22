namespace Logitar.Identity.Core.Models;

public abstract record Aggregate
{
  public string Id { get; set; } = string.Empty;

  public Actor CreatedBy { get; set; } = new();
  public DateTime CreatedOn { get; set; }

  public Actor UpdatedBy { get; set; } = new();
  public DateTime UpdatedOn { get; set; }

  public long Version { get; set; }
}
