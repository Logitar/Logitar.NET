namespace Logitar.Data;

/// <summary>
/// Represents the identifier of data column.
/// </summary>
public record ColumnId
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ColumnId"/> class.
  /// </summary>
  /// <param name="name">The name of the column.</param>
  /// <param name="table">The table in which the column is.</param>
  /// <param name="alias">The alias of the column.</param>
  /// <exception cref="ArgumentException">The column name was missing.</exception>
  public ColumnId(string name, TableId? table = null, string? alias = null) : this(table)
  {
    if (string.IsNullOrWhiteSpace(name))
    {
      throw new ArgumentException("The column name is required.", nameof(name));
    }

    Name = name.Trim();
    Alias = alias?.CleanTrim();
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="ColumnId"/> class.
  /// </summary>
  /// <param name="table">The table in which the column is.</param>
  private ColumnId(TableId? table = null)
  {
    Table = table;
  }

  /// <summary>
  /// Gets the name of the column.
  /// </summary>
  public string? Name { get; }
  /// <summary>
  /// Gets the alias of the column.
  /// </summary>
  public string? Alias { get; }
  /// <summary>
  /// Gets the table in which the column is.
  /// </summary>
  public TableId? Table { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="ColumnId"/> class, specifying all columns.
  /// <br />This is useful for select all columns of a table, or all columns from the query results.
  /// </summary>
  /// <param name="table">The table in which the column is.</param>
  /// <returns>The column identifier.</returns>
  public static ColumnId All(TableId? table = null) => new(table);
}
