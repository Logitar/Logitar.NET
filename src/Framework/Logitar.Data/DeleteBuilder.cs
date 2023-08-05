namespace Logitar.Data;

/// <summary>
/// Represents an abstraction of a generic SQL delete command builder, to be used by specific implementations.
/// </summary>
public abstract class DeleteBuilder : IDeleteBuilder
{
  /// <summary>
  /// Initializes a new instance of the <see cref="DeleteBuilder"/> class.
  /// </summary>
  /// <param name="source">The source table.</param>
  /// <exception cref="ArgumentException">The source table name has not been specified.</exception>
  protected DeleteBuilder(TableId source)
  {
    if (source.Table == null)
    {
      throw new ArgumentException("The table name is required.", nameof(source));
    }

    Source = source;
  }

  /// <summary>
  /// Gets the table in which the command will delete data from.
  /// </summary>
  protected TableId Source { get; }
  /// <summary>
  /// Gets the list of parameters of the command.
  /// </summary>
  protected ICollection<IParameter> Parameters { get; } = new List<IParameter>();
  /// <summary>
  /// Gets the list of conditions of the command.
  /// </summary>
  protected ICollection<Condition> Conditions { get; } = new List<Condition>();

  /// <summary>
  /// Gets the default schema of the generic dialect.
  /// </summary>
  protected virtual string? DefaultSchema => null;
  /// <summary>
  /// Gets the prefix of identifiers in the generic dialect.
  /// </summary>
  protected virtual string? IdentifierPrefix => null;
  /// <summary>
  /// Gets the suffix of identifiers in the generic dialect.
  /// </summary>
  protected virtual string? IdentifierSuffix => null;
  /// <summary>
  /// Gets the identifier separator in the generic dialect.
  /// </summary>
  protected virtual string IdentifierSeparator => ".";
  /// <summary>
  /// Gets the prefix of parameters in the generic dialect.
  /// </summary>
  protected virtual string? ParameterPrefix => "@";
  /// <summary>
  /// Gets the suffix of parameters in the generic dialect.
  /// </summary>
  protected virtual string? ParameterSuffix => null;

  /// <summary>
  /// Gets the all-columns (*) clause in the generic dialect.
  /// </summary>
  protected virtual string AllColumnsClause => "*";

  /// <summary>
  /// Gets the DELETE FROM clause in the generic dialect.
  /// </summary>
  protected virtual string DeleteFromClause => "DELETE FROM";

  /// <summary>
  /// Gets the WHERE clause in the generic dialect.
  /// </summary>
  protected virtual string WhereClause => "WHERE";
  /// <summary>
  /// Gets the IS clause in the generic dialect.
  /// </summary>
  protected virtual string IsClause => "IS";
  /// <summary>
  /// Gets the NOT clause in the generic dialect.
  /// </summary>
  protected virtual string NotClause => "NOT";
  /// <summary>
  /// Gets the BETWEEN clause in the generic dialect.
  /// </summary>
  protected virtual string BetweenClause => "BETWEEN";
  /// <summary>
  /// Gets the IN clause in the generic dialect.
  /// </summary>
  protected virtual string InClause => "IN";
  /// <summary>
  /// Gets the LIKE clause in the generic dialect.
  /// </summary>
  protected virtual string LikeClause => "LIKE";
  /// <summary>
  /// Gets the NULL clause in the generic dialect.
  /// </summary>
  protected virtual string NullClause => "NULL";
  /// <summary>
  /// Gets the comparison operators of the current dialect.
  /// </summary>
  protected virtual Dictionary<string, string> ComparisonOperators { get; } = new();
  /// <summary>
  /// Gets the group operators of the current dialect.
  /// </summary>
  protected virtual Dictionary<string, string> GroupOperators { get; } = new();

  /// <summary>
  /// Applies the specified conditions to the command.
  /// </summary>
  /// <param name="conditions">The conditions to apply.</param>
  /// <returns>The command builder.</returns>
  public IDeleteBuilder Where(params Condition[] conditions)
  {
    Conditions.AddRange(conditions);
    return this;
  }

  /// <summary>
  /// Builds the delete data command.
  /// </summary>
  /// <returns>The data command.</returns>
  public ICommand Build()
  {
    StringBuilder text = new();

    text.Append(DeleteFromClause).Append(' ').AppendLine(Format(Source));

    if (Conditions.Any())
    {
      _ = GroupOperators.TryGetValue("AND", out string? andOperator);
      andOperator ??= "AND";

      text.Append(WhereClause).Append(' ').AppendLine(string.Join($" {andOperator} ", Conditions.Select(Format)));
    }

    IEnumerable<object> parameters = Parameters.Select(CreateParameter);

    return new Command(text.ToString().TrimEnd(','), parameters);
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
        _ = GroupOperators.TryGetValue(group.Operator, out string? groupOperator);
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

    _ = GroupOperators.TryGetValue("AND", out string? andOperator);
    andOperator ??= "AND";

    if (between.NotBetween)
    {
      formatted.Append(NotClause).Append(' ');
    }

    formatted.Append(BetweenClause).Append(' ').Append(Format(AddParameter(between.MinValue)))
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
    _ = ComparisonOperators.TryGetValue(comparison.Operator, out string? comparisonOperator);
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
      formatted.Append(NotClause).Append(' ');
    }

    formatted.Append(InClause).Append(" (")
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
      formatted.Append(NotClause).Append(' ');
    }

    formatted.Append(LikeClause).Append(' ').Append(Format(AddParameter(like.Pattern)));

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

    formatted.Append(IsClause).Append(' ');

    if (@null.NotNull)
    {
      formatted.Append(NotClause).Append(' ');
    }

    formatted.Append(NullClause);

    return formatted.ToString();
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
      formatted.Append(Format(column.Table)).Append(IdentifierSeparator);
    }

    formatted.Append(column.Name == null ? AllColumnsClause : Format(column.Name));

    return formatted.ToString();
  }
  /// <summary>
  /// Formats the specified table identifier to SQL.
  /// </summary>
  /// <param name="table">The table identifier to format.</param>
  /// <returns>The formatted SQL.</returns>
  protected virtual string Format(TableId table)
  {
    StringBuilder formatted = new();

    string? schema = table.Schema ?? DefaultSchema;
    if (schema != null)
    {
      formatted.Append(Format(schema)).Append(IdentifierSeparator);
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
    return string.Concat(ParameterPrefix, parameter.Name, ParameterSuffix);
  }

  /// <summary>
  /// Formats the specified identifier to SQL.
  /// </summary>
  /// <param name="identifier">The identifier to format.</param>
  /// <returns>The formatted SQL.</returns>
  protected virtual string Format(string identifier)
  {
    return string.Concat(IdentifierPrefix, identifier, IdentifierSuffix);
  }

  /// <summary>
  /// Creates a new implementation-specific command parameter.
  /// </summary>
  /// <param name="parameter">The parameter information.</param>
  /// <returns>The implementation-specific parameter.</returns>
  protected abstract object CreateParameter(IParameter parameter);
}
