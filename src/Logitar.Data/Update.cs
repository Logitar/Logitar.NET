namespace Logitar.Data;

/// <summary>
/// Represents a value update for a column.
/// </summary>
public record Update
{
  /// <summary>
  /// Initializes a new instance of the <see cref="Update"/> class.
  /// </summary>
  /// <param name="column">The column to update.</param>
  /// <param name="value">The new value of the column.</param>
  /// <exception cref="ArgumentException"></exception>
  public Update(ColumnId column, object? value)
  {
    if (column.Name == null)
    {
      throw new ArgumentException("The column name is required.", nameof(column));
    }

    Column = column;
    Value = value;
  }

  /// <summary>
  /// Gets the column to update.
  /// </summary>
  public ColumnId Column { get; }
  /// <summary>
  /// Gets the new value of the column.
  /// </summary>
  public object? Value { get; }
}
