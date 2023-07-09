namespace Logitar.Data;

/// <summary>
/// TODO(fpion): document
/// </summary>
public abstract class QueryBuilder : IQueryBuilder
{
  protected QueryBuilder(TableId source)
  {
    if (source.Table == null)
    {
      throw new ArgumentException("The table name is required.", nameof(source));
    }

    Source = source;
  }

  protected TableId Source { get; }
  protected ICollection<IParameter> Parameters { get; } = new List<IParameter>();
  protected ICollection<ColumnId> Selections { get; } = new List<ColumnId>();
  protected ICollection<Condition> Conditions { get; } = new List<Condition>();
  protected ICollection<OrderBy> OrderByList { get; } = new List<OrderBy>();

  protected virtual string? DefaultSchema => null;
  protected virtual string? IdentifierPrefix => null;
  protected virtual string? IdentifierSuffix => null;
  protected virtual string IdentifierSeparator => ".";
  protected virtual string? ParameterPrefix => null;
  protected virtual string? ParameterSuffix => null;

  protected virtual string SelectClause => "SELECT";
  protected virtual string AllColumnsClause => "*";

  protected virtual string FromClause => "FROM";

  protected virtual string WhereClause => "WHERE";
  protected virtual string IsClause => "IS";
  protected virtual string NotClause => "NOT";
  protected virtual string BetweenClause => "BETWEEN";
  protected virtual string InClause => "IN";
  protected virtual string LikeClause => "LIKE";
  protected virtual string NullClause => "NULL";
  protected virtual Dictionary<string, string> ComparisonOperators { get; } = new();
  protected virtual Dictionary<string, string> GroupOperators { get; } = new();

  protected virtual string OrderByClause => "ORDER BY";
  protected virtual string ThenByClause => "THEN BY";
  protected virtual string AscendingClause => "ASC";
  protected virtual string DescendingClause => "DESC";

  public IQueryBuilder Select(params ColumnId[] columns)
  {
    Selections.AddRange(columns);
    return this;
  }

  public IQueryBuilder Where(params Condition[] conditions)
  {
    Conditions.AddRange(conditions);
    return this;
  }

  public IQueryBuilder OrderBy(params OrderBy[] orderBy)
  {
    OrderByList.Clear();
    OrderByList.AddRange(orderBy);
    return this;
  }

  public IQuery Build()
  {
    StringBuilder text = new();

    if (Selections.Any())
    {
      text.Append(SelectClause).Append(' ').AppendLine(string.Join(", ", Selections.Select(Format)));
    }

    text.Append(FromClause).Append(' ').AppendLine(Format(Source, fullName: true));

    if (Conditions.Any())
    {
      _ = GroupOperators.TryGetValue("AND", out string? andOperator);
      andOperator ??= "AND";

      text.Append(WhereClause).Append(' ').AppendLine(string.Join($" {andOperator} ", Conditions.Select(Format)));
    }

    if (OrderByList.Any())
    {
      text.Append(OrderByClause).Append(' ').AppendLine(string.Join($" {ThenByClause} ", OrderByList.Select(Format)));
    }

    IEnumerable<object> parameters = Parameters.Select(CreateParameter);

    return new Query(text.ToString(), parameters);
  }

  protected virtual string Format(Condition condition)
  {
    if (condition is OperatorCondition @operator)
    {
      return string.Join(' ', Format(@operator.Column), Format(@operator.Operator));
    }
    else if (condition is ConditionGroup group)
    {
      _ = GroupOperators.TryGetValue(group.Operator, out string? groupOperator);
      groupOperator ??= group.Operator;

      return string.Concat('(', string.Join($" {groupOperator} ", group.Conditions.Select(Format)), ')');
    }

    throw new NotSupportedException($"The condition '{condition}' is not supported.");
  }
  protected virtual string Format(ConditionalOperator @operator)
  {
    if (@operator is BetweenOperator between)
    {
      return Format(between);
    }
    else if (@operator is ComparisonOperator comparison)
    {
      return Format(comparison);
    }
    else if (@operator is InOperator @in)
    {
      return Format(@in);
    }
    else if (@operator is LikeOperator like)
    {
      return Format(like);
    }
    else if (@operator is NullOperator @null)
    {
      return Format(@null);
    }
    else
    {
      throw new NotSupportedException($"The conditional operator '{@operator}' is not supported.");
    }
  }
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
  protected virtual string Format(ComparisonOperator comparison)
  {
    _ = ComparisonOperators.TryGetValue(comparison.Operator, out string? comparisonOperator);
    comparisonOperator ??= comparison.Operator;

    return string.Join(' ', comparisonOperator, Format(AddParameter(comparison.Value)));
  }
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

  protected virtual string Format(OrderBy orderBy)
  {
    return string.Join(' ', Format(orderBy.Column), orderBy.IsDescending ? DescendingClause : AscendingClause);
  }

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
  protected virtual string Format(TableId table, bool fullName = false)
  {
    if (!fullName && table.Alias != null)
    {
      return Format(table.Alias);
    }

    StringBuilder formatted = new();

    string? schema = table.Schema ?? DefaultSchema;
    if (schema != null)
    {
      formatted.Append(Format(schema)).Append(IdentifierSeparator);
    }

    formatted.Append(Format(table.Table ?? string.Empty));

    if (fullName && table.Alias != null)
    {
      formatted.Append(' ').Append(Format(table.Alias));
    }

    return formatted.ToString();
  }

  protected virtual IParameter AddParameter(object value)
  {
    Parameter parameter = new(string.Concat('p', Parameters.Count), value);
    Parameters.Add(parameter);

    return parameter;
  }
  protected virtual string Format(IParameter parameter)
  {
    return string.Concat(ParameterPrefix, parameter.Name, ParameterSuffix);
  }

  protected virtual string Format(string identifier)
  {
    return string.Concat(IdentifierPrefix, identifier, IdentifierSuffix);
  }

  protected abstract object CreateParameter(IParameter parameter);
}
