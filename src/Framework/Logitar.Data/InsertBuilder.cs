namespace Logitar.Data;

/// <summary>
/// Represents a generic SQL insert command builder, to be used by specific implementations.
/// </summary>
public class InsertBuilder : SqlBuilder, IInsertBuilder
{
  /// <summary>
  /// Initializes a new instance of the <see cref="InsertBuilder"/> class.
  /// </summary>
  /// <param name="columns">The columns to insert into.</param>
  /// <exception cref="ArgumentException">No column has been specified, or a column name or table has not been specified, or multiple tables have been specified.</exception>
  protected InsertBuilder(params ColumnId[] columns)
  {
    if (!columns.Any())
    {
      throw new ArgumentException("At least one column must be specified.", nameof(columns));
    }

    HashSet<TableId> tables = new(capacity: columns.Length);
    for (int i = 0; i < columns.Length; i++)
    {
      ColumnId column = columns[i];
      if (column.Name == null)
      {
        throw new ArgumentException($"The column name is required (index: {i}).", nameof(columns));
      }
      else if (column.Table == null)
      {
        throw new ArgumentException($"The column table cannot be null (index: {i}).", nameof(columns));
      }

      tables.Add(column.Table);
    }
    if (tables.Count > 1)
    {
      throw new ArgumentException("An insert command cannot insert into multiple tables.", nameof(columns));
    }
    Table = tables.Single();
    Columns = columns;
  }

  /// <summary>
  /// Gets the table in which the command will insert data into.
  /// </summary>
  protected TableId Table { get; }
  /// <summary>
  /// Gets the columns in which the command will insert data into.
  /// </summary>
  protected IEnumerable<ColumnId> Columns { get; }
  /// <summary>
  /// Gets the list of rows that will be inserted by the command.
  /// </summary>
  protected ICollection<object?[]> Rows { get; } = new List<object?[]>();

  /// <summary>
  /// Inserts the specified row values in the command builder.
  /// </summary>
  /// <param name="values">The row values to insert.</param>
  /// <returns>The command builder.</returns>
  /// <exception cref="ArgumentException">No value has been specified.</exception>
  public IInsertBuilder Value(params object?[] values)
  {
    if (!values.Any())
    {
      throw new ArgumentException("At least one value must be inserted.", nameof(values));
    }

    Rows.Add(values);
    return this;
  }

  /// <summary>
  /// Builds the insert data command.
  /// </summary>
  /// <returns>The data command.</returns>
  public ICommand Build()
  {
    if (!Rows.Any())
    {
      throw new InvalidOperationException("At least one row must be inserted.");
    }

    StringBuilder text = new();

    text.Append(Dialect.InsertIntoClause).Append(' ').Append(Format(Table)).Append(" (")
      .Append(string.Join(", ", Columns.Select(column => Format(column.Name!))))
      .Append(") ").AppendLine(Dialect.ValuesClause);

    for (int i = 0; i < Rows.Count; i++)
    {
      object?[] row = Rows.ElementAt(i);

      text.Append('(')
        .Append(string.Join(", ", row.Select(cell => cell == null ? Dialect.NullClause : Format(AddParameter(cell)))))
        .Append(')');

      if (i < (Rows.Count - 1))
      {
        text.Append(',').AppendLine();
      }
    }

    IEnumerable<object> parameters = Parameters.Select(Dialect.CreateParameter);

    return new Command(text.ToString().TrimEnd(','), parameters);
  }
}
