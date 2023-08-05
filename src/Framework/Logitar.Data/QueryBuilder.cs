namespace Logitar.Data;

/// <summary>
/// Represents an abstraction of a generic SQL query builder, to be used by specific implementations.
/// </summary>
public abstract class QueryBuilder : IQueryBuilder
{
  /// <summary>
  /// Initializes a new instance of the <see cref="QueryBuilder"/> class.
  /// </summary>
  /// <param name="source">The source table.</param>
  /// <exception cref="ArgumentException">The source table name is missing.</exception>
  protected QueryBuilder(TableId source)
  {
    if (source.Table == null)
    {
      throw new ArgumentException("The table name is required.", nameof(source));
    }

    Source = source;
  }

  /// <summary>
  /// Gets or sets the dialect used to format to SQL.
  /// </summary>
  public virtual Dialect Dialect { get; set; } = new();

  /// <summary>
  /// Gets the source table of the query.
  /// </summary>
  protected TableId Source { get; }
  /// <summary>
  /// Gets the list of parameters of the query.
  /// </summary>
  protected ICollection<IParameter> Parameters { get; } = new List<IParameter>();
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
      text.Append(Dialect.SelectClause).Append(' ').AppendLine(string.Join(", ", Selections.Select(Format)));
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
        .AppendLine(string.Join($" {Dialect.ThenByClause} ", OrderByList.Select(Format)));
    }

    IEnumerable<object> parameters = Parameters.Select(CreateParameter);

    return new Query(text.ToString(), parameters);
  }

  /// <summary>
  /// Formats the specified join to SQL.
  /// </summary>
  /// <param name="join">The join to format.</param>
  /// <returns>The formatted SQL.</returns>
  protected virtual string Format(Join join)
  {
    StringBuilder formatted = new();

    if (!Dialect.JoinClauses.TryGetValue(join.Kind, out string? joinClause))
    {
      joinClause = $"{join.Kind.ToString().ToUpper()} JOIN";
    }
    if (!Dialect.ComparisonOperators.TryGetValue("=", out string? equalOperator))
    {
      equalOperator = "=";
    }

    formatted.Append(joinClause).Append(' ').Append(Format(join.Left.Table!, fullName: true))
      .Append(' ').Append(Dialect.OnClause).Append(' ').Append(Format(join.Left)).Append(' ')
      .Append(equalOperator).Append(' ').Append(Format(join.Right));

    if (join.Condition != null)
    {
      _ = Dialect.GroupOperators.TryGetValue("AND", out string? andOperator);
      andOperator ??= "AND";

      formatted.Append(' ').Append(andOperator).Append(' ').Append(Format(join.Condition));
    }

    return formatted.ToString();
  }

  /// <summary>
  /// Formats the specified condition to SQL.
  /// </summary>
  /// <param name="condition">The condition to format.</param>
  /// <returns>The formatted SQL.</returns>
  /// <exception cref="NotSupportedException">The condition type is not supported.</exception>
  protected virtual string Format(Condition condition)
  {
    switch (condition)
    {
      case OperatorCondition @operator:
        return string.Join(' ', Format(@operator.Column), Format(@operator.Operator));
      case ConditionGroup group:
        _ = Dialect.GroupOperators.TryGetValue(group.Operator, out string? groupOperator);
        groupOperator ??= group.Operator;
        return string.Concat('(', string.Join($" {groupOperator} ", group.Conditions.Select(Format)), ')');
      default:
        throw new NotSupportedException($"The condition '{condition}' is not supported.");
    }
  }
  /// <summary>
  /// Formats the specified conditional operator to SQL.
  /// </summary>
  /// <param name="operator">The operator to format.</param>
  /// <returns>The formatted SQL.</returns>
  /// <exception cref="NotSupportedException">The conditional operator type is not supported.</exception>
  protected virtual string Format(ConditionalOperator @operator)
  {
    return @operator switch
    {
      BetweenOperator between => Format(between),
      ComparisonOperator comparison => Format(comparison),
      InOperator @in => Format(@in),
      LikeOperator like => Format(like),
      NullOperator @null => Format(@null),
      _ => throw new NotSupportedException($"The conditional operator '{@operator}' is not supported."),
    };
  }
  /// <summary>
  /// Formats the specified BETWEEN operator to SQL.
  /// </summary>
  /// <param name="between">The operator to format.</param>
  /// <returns>The formatted SQL.</returns>
  protected virtual string Format(BetweenOperator between)
  {
    StringBuilder formatted = new();

    _ = Dialect.GroupOperators.TryGetValue("AND", out string? andOperator);
    andOperator ??= "AND";

    if (between.NotBetween)
    {
      formatted.Append(Dialect.NotClause).Append(' ');
    }

    formatted.Append(Dialect.BetweenClause).Append(' ').Append(Format(AddParameter(between.MinValue)))
      .Append($" {andOperator} ").Append(Format(AddParameter(between.MaxValue)));

    return formatted.ToString();
  }
  /// <summary>
  /// Formats the specified comparison operator to SQL.
  /// </summary>
  /// <param name="comparison">The operator to format.</param>
  /// <returns>The formatted SQL.</returns>
  protected virtual string Format(ComparisonOperator comparison)
  {
    _ = Dialect.ComparisonOperators.TryGetValue(comparison.Operator, out string? comparisonOperator);
    comparisonOperator ??= comparison.Operator;

    return string.Join(' ', comparisonOperator, Format(AddParameter(comparison.Value)));
  }
  /// <summary>
  /// Formats the specified IN operator to SQL.
  /// </summary>
  /// <param name="in">The operator to format.</param>
  /// <returns>The formatted SQL.</returns>
  protected virtual string Format(InOperator @in)
  {
    StringBuilder formatted = new();

    if (@in.NotIn)
    {
      formatted.Append(Dialect.NotClause).Append(' ');
    }

    formatted.Append(Dialect.InClause).Append(" (")
      .Append(string.Join(", ", @in.Values.Select(value => Format(AddParameter(value)))))
      .Append(')');

    return formatted.ToString();
  }
  /// <summary>
  /// Formats the specified LIKE operator to SQL.
  /// </summary>
  /// <param name="like">The operator to format.</param>
  /// <returns>The formatted SQL.</returns>
  protected virtual string Format(LikeOperator like)
  {
    StringBuilder formatted = new();

    if (like.NotLike)
    {
      formatted.Append(Dialect.NotClause).Append(' ');
    }

    formatted.Append(Dialect.LikeClause).Append(' ').Append(Format(AddParameter(like.Pattern)));

    return formatted.ToString();
  }
  /// <summary>
  /// Formats the specified NULL operator to SQL.
  /// </summary>
  /// <param name="null">The operator to format.</param>
  /// <returns>The formatted SQL.</returns>
  protected virtual string Format(NullOperator @null)
  {
    StringBuilder formatted = new();

    formatted.Append(Dialect.IsClause).Append(' ');

    if (@null.NotNull)
    {
      formatted.Append(Dialect.NotClause).Append(' ');
    }

    formatted.Append(Dialect.NullClause);

    return formatted.ToString();
  }

  /// <summary>
  /// Formats the specified sort parameter to SQL.
  /// </summary>
  /// <param name="orderBy">The sort parameter to format.</param>
  /// <returns>The formatted SQL.</returns>
  protected virtual string Format(OrderBy orderBy)
  {
    return string.Join(' ', Format(orderBy.Column), orderBy.IsDescending ? Dialect.DescendingClause : Dialect.AscendingClause);
  }

  /// <summary>
  /// Formats the specified column identifier to SQL.
  /// </summary>
  /// <param name="column">The column identifier to format.</param>
  /// <returns>The formatted SQL.</returns>
  protected virtual string Format(ColumnId column)
  {
    StringBuilder formatted = new();

    if (column.Table != null)
    {
      formatted.Append(Format(column.Table)).Append(Dialect.IdentifierSeparator);
    }

    formatted.Append(column.Name == null ? Dialect.AllColumnsClause : Format(column.Name));

    return formatted.ToString();
  }
  /// <summary>
  /// Formats the specified table identifier to SQL.
  /// </summary>
  /// <param name="table">The table identifier to format.</param>
  /// <param name="fullName">If true, the full table identifier, including schema, table and alias, will be returned. Else, only the alias or schema and table will be returned.</param>
  /// <returns>The formatted SQL.</returns>
  protected virtual string Format(TableId table, bool fullName = false)
  {
    if (!fullName && table.Alias != null)
    {
      return Format(table.Alias);
    }

    StringBuilder formatted = new();

    string? schema = table.Schema ?? Dialect.DefaultSchema;
    if (schema != null)
    {
      formatted.Append(Format(schema)).Append(Dialect.IdentifierSeparator);
    }

    formatted.Append(Format(table.Table ?? string.Empty));

    if (fullName && table.Alias != null)
    {
      formatted.Append(' ').Append(Format(table.Alias));
    }

    return formatted.ToString();
  }

  /// <summary>
  /// Adds a new parameter to the query.
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
  /// Creates a new implementation-specific query parameter.
  /// </summary>
  /// <param name="parameter">The parameter information.</param>
  /// <returns>The implementation-specific parameter.</returns>
  protected abstract object CreateParameter(IParameter parameter);
}
