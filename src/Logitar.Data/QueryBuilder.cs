namespace Logitar.Data;

/// <summary>
/// Represents a generic SQL query builder, to be used by specific implementations.
/// </summary>
public class QueryBuilder : SqlBuilder, IQueryBuilder
{
  /// <summary>
  /// Initializes a new instance of the <see cref="QueryBuilder"/> class.
  /// </summary>
  /// <param name="source">The source table.</param>
  /// <exception cref="ArgumentException">The source table name is missing.</exception>
  public QueryBuilder(TableId source)
  {
    if (source.Table == null)
    {
      throw new ArgumentException("The table name is required.", nameof(source));
    }

    Source = source;
  }

  /// <summary>
  /// Gets the source table of the query.
  /// </summary>
  protected TableId Source { get; }
  /// <summary>
  /// Gets the list of selections of the query.
  /// </summary>
  protected ICollection<ColumnId> Selections { get; } = new List<ColumnId>();
  /// <summary>
  /// Gets the list of joins of the query.
  /// </summary>
  protected ICollection<Join> Joins { get; } = new List<Join>();
  /// <summary>
  /// Gets the list of conditions of the query.
  /// </summary>
  protected ICollection<Condition> Conditions { get; } = new List<Condition>();
  /// <summary>
  /// Gets the list of sort parameters of the query.
  /// </summary>
  protected ICollection<OrderBy> OrderByList { get; } = new List<OrderBy>();

  /// <summary>
  /// Selects the specified columns in the query results.
  /// </summary>
  /// <param name="columns">The columns to select.</param>
  /// <returns>The query builder.</returns>
  public IQueryBuilder Select(params ColumnId[] columns)
  {
    Selections.AddRange(columns);
    return this;
  }

  /// <summary>
  /// Applies the specified joins to the query.
  /// </summary>
  /// <param name="joins">The joins to apply.</param>
  /// <returns>The query builder.</returns>
  public IQueryBuilder Join(params Join[] joins)
  {
    Joins.AddRange(joins);
    return this;
  }

  /// <summary>
  /// Applies the specified conditions to the query.
  /// </summary>
  /// <param name="conditions">The conditions to apply.</param>
  /// <returns>The query builder.</returns>
  public IQueryBuilder Where(params Condition[] conditions)
  {
    Conditions.AddRange(conditions);
    return this;
  }

  /// <summary>
  /// Applies the specified sort parameters to the query.
  /// </summary>
  /// <param name="orderBy">The sort parameters to apply.</param>
  /// <returns>The query builder.</returns>
  public IQueryBuilder OrderBy(params OrderBy[] orderBy)
  {
    OrderByList.Clear();
    OrderByList.AddRange(orderBy);
    return this;
  }

  /// <summary>
  /// Builds the SQL query.
  /// </summary>
  /// <returns>The SQL query.</returns>
  public IQuery Build()
  {
    StringBuilder text = new();

    if (Selections.Any())
    {
      text.Append(Dialect.SelectClause).Append(' ').AppendLine(string.Join(", ", Selections.Select(x => Format(x, fullName: true))));
    }

    text.Append(Dialect.FromClause).Append(' ').AppendLine(Format(Source, fullName: true));

    if (Joins.Any())
    {
      foreach (Join join in Joins)
      {
        text.AppendLine(Format(join));
      }
    }

    if (Conditions.Any())
    {
      _ = Dialect.GroupOperators.TryGetValue("AND", out string? andOperator);
      andOperator ??= "AND";

      text.Append(Dialect.WhereClause).Append(' ')
        .AppendLine(string.Join($" {andOperator} ", Conditions.Select(Format)));
    }

    if (OrderByList.Any())
    {
      text.Append(Dialect.OrderByClause).Append(' ')
        .AppendLine(string.Join(", ", OrderByList.Select(Format)));
    }

    IEnumerable<object> parameters = Parameters.Select(Dialect.CreateParameter);

    return new Query(text.ToString(), parameters);
  }
}
