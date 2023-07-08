namespace Logitar.Data;

/// <summary>
/// TODO(fpion): document
/// </summary>
public record TableId
{
  public TableId(string? schema, string table, string? alias = null)
  {
    if (string.IsNullOrWhiteSpace(table))
    {
      throw new ArgumentException("The table name is required.", nameof(table));
    }

    Schema = schema?.CleanTrim();
    Table = table.Trim();
    Alias = alias?.CleanTrim();
  }

  public string? Schema { get; }
  public string Table { get; }
  public string? Alias { get; }
}
