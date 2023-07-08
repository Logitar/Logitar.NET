namespace Logitar.Data;

/// <summary>
/// TODO(fpion): document
/// </summary>
public record OrderBy
{
  public OrderBy(ColumnId column, bool isDescending = false)
  {
    if (column.Name == null)
    {
      throw new ArgumentException("The column name is required.", nameof(column));
    }

    Column = column;
    IsDescending = isDescending;
  }

  public ColumnId Column { get; }
  public bool IsDescending { get; }
}
