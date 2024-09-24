namespace Logitar.Data;

/// <summary>
/// Represents an abstraction of a generic SQL command/query builder, to be used by specific implementations.
/// </summary>
public abstract class SqlBuilder
{
  /// <summary>
  /// Gets or sets the dialect used to format to SQL.
  /// </summary>
  public virtual Dialect Dialect { get; set; } = new();
  /// <summary>
  /// Gets the list of parameters of the command/query.
  /// </summary>
  protected virtual ICollection<IParameter> Parameters { get; } = new List<IParameter>();

  /// <summary>
  /// Formats the specified column update to SQL.
  /// </summary>
  /// <param name="update">The column update to format.</param>
  /// <returns>The formatted SQL.</returns>
  protected virtual string Format(Update update)
  {
    if (!Dialect.ComparisonOperators.TryGetValue("=", out string? equalOperator))
    {
      equalOperator = "=";
    }

    string newValue = update.Value == null ? Dialect.NullClause : Format(AddParameter(update.Value));

    return string.Join(' ', Format(update.Column), equalOperator, newValue);
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

    formatted.Append(joinClause).Append(' ').Append(Format(join.Right.Table!, fullName: true))
      .Append(' ').Append(Dialect.OnClause).Append(' ').Append(Format(join.Right)).Append(' ')
      .Append(equalOperator).Append(' ').Append(Format(join.Left));

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
  /// <param name="fullName">If true, the full column identifier, including the table name and schema or alias, column name and column alias will be returned. Else, only the alias, or column table and name will be returned.</param>
  /// <returns>The formatted SQL.</returns>
  protected virtual string Format(ColumnId column, bool fullName = false)
  {
    if (!fullName && column.Alias != null)
    {
      return Format(column.Alias);
    }

    StringBuilder formatted = new();

    if (column.Table != null)
    {
      formatted.Append(Format(column.Table)).Append(Dialect.IdentifierSeparator);
    }

    formatted.Append(column.Name == null ? Dialect.AllColumnsClause : Format(column.Name));

    if (column.Alias != null)
    {
      formatted.Append(' ').Append(Dialect.AsClause).Append(' ').Append(Format(column.Alias));
    }

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
  /// Adds a new parameter to the command/query.
  /// </summary>
  /// <param name="value">The value of the parameter.</param>
  /// <returns>The new parameter.</returns>
  protected virtual IParameter AddParameter(object value)
  {
    Parameter parameter = new(string.Concat('p', Parameters.Count), value);
    Parameters.Add(parameter);

    return parameter;
  }
}
