namespace Logitar.Data;

/// <summary>
/// TODO(fpion): document
/// </summary>
public class ColumnId
{
  public ColumnId(string name, TableId? table = null) : this(table)
  {
    if (string.IsNullOrWhiteSpace(name))
    {
      throw new ArgumentException("The column name is required.", nameof(name));
    }

    Name = name.Trim();
  }
  private ColumnId(TableId? table = null)
  {
    Table = table;
  }

  public string? Name { get; }
  public TableId? Table { get; }

  public static ColumnId All(TableId? table = null) => new(table);
}
