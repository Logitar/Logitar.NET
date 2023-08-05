namespace Logitar.Data;

/// <summary>
/// Represents an abstraction of a generic SQL insert command builder, to be used by specific implementations.
/// </summary>
public abstract class InsertBuilder : IInsertBuilder
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
  /// Gets or sets the dialect used to format to SQL.
  /// </summary>
  public virtual Dialect Dialect { get; set; } = new();

  /// <summary>
  /// Gets the table in which the command will insert data into.
  /// </summary>
  protected TableId Table { get; }
  /// <summary>
  /// Gets the columns in which the command will insert data into.
  /// </summary>
  protected IEnumerable<ColumnId> Columns { get; }
  /// <summary>
  /// Gets the list of parameters of the command.
  /// </summary>
  protected ICollection<IParameter> Parameters { get; } = new List<IParameter>();
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

    IEnumerable<object> parameters = Parameters.Select(CreateParameter);

    return new Command(text.ToString().TrimEnd(','), parameters);
  }

  /// <summary>
  /// Formats the specified table identifier to SQL.
  /// </summary>
  /// <param name="table">The table identifier to format.</param>
  /// <returns>The formatted SQL.</returns>
  protected virtual string Format(TableId table)
  {
    StringBuilder formatted = new();

    string? schema = table.Schema ?? Dialect.DefaultSchema;
    if (schema != null)
    {
      formatted.Append(Format(schema)).Append(Dialect.IdentifierSeparator);
    }

    formatted.Append(Format(table.Table ?? string.Empty));

    return formatted.ToString();
  }

  /// <summary>
  /// Adds a new parameter to the command.
  /// </summary>
  /// <param name="value">The value of the parameter.</param>
  /// <returns>The new parameter.</returns>
  protected virtual IParameter AddParameter(object value)
  {
    Parameter parameter = new(string.Concat('p', Parameters.Count), value);
    Parameters.Add(parameter);

    return parameter;
  }
  /// <summary>
  /// Formats the specified parameter name to SQL.
  /// </summary>
  /// <param name="parameter">The parameter to format.</param>
  /// <returns>The formatted SQL.</returns>
  protected virtual string Format(IParameter parameter)
  {
    return string.Concat(Dialect.ParameterPrefix, parameter.Name, Dialect.ParameterSuffix);
  }

  /// <summary>
  /// Formats the specified identifier to SQL.
  /// </summary>
  /// <param name="identifier">The identifier to format.</param>
  /// <returns>The formatted SQL.</returns>
  protected virtual string Format(string identifier)
  {
    return string.Concat(Dialect.IdentifierPrefix, identifier, Dialect.IdentifierSuffix);
  }

  /// <summary>
  /// Creates a new implementation-specific command parameter.
  /// </summary>
  /// <param name="parameter">The parameter information.</param>
  /// <returns>The implementation-specific parameter.</returns>
  protected abstract object CreateParameter(IParameter parameter);
}
