namespace Logitar.Data;

/// <summary>
/// Represents a sort parameter of a data query.
/// </summary>
public record OrderBy
{
  /// <summary>
  /// Initializes a new instance of the <see cref="OrderBy"/> class.
  /// </summary>
  /// <param name="column">The column to sort by.</param>
  /// <param name="isDescending">A value indicating whether or not the sort is descending.</param>
  /// <exception cref="ArgumentException">The column name was missing.</exception>
  public OrderBy(ColumnId column, bool isDescending = false)
  {
    if (column.Name == null)
    {
      throw new ArgumentException("The column name is required.", nameof(column));
    }

    Column = column;
    IsDescending = isDescending;
  }

  /// <summary>
  /// Gets the column to sort by.
  /// </summary>
  public ColumnId Column { get; }
  /// <summary>
  /// Gets a value indicating whether or not the sort is descending.
  /// </summary>
  public bool IsDescending { get; }
}
