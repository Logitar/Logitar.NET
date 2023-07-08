namespace Logitar.Data;

/// <summary>
/// TODO(fpion): document
/// </summary>
public class QueryBuilder : IQueryBuilder
{
  private readonly TableId _source;
  private readonly List<ColumnId> _selections = new();
  private readonly List<OrderBy> _orderBy = new();

  public QueryBuilder(TableId source)
  {
    if (source.Table == null)
    {
      throw new ArgumentException("The table name is required.", nameof(source));
    }

    _source = source;
  }

  protected virtual string? DefaultSchema => null;
  protected virtual string? IdentifierPrefix => null;
  protected virtual string? IdentifierSuffix => null;
  protected virtual string IdentifierSeparator => ".";

  protected virtual string SelectClause => "SELECT";
  protected virtual string AllColumnsClause => "*";

  protected virtual string FromClause => "FROM";

  protected virtual string OrderByClause => "ORDER BY";
  protected virtual string ThenByClause => "THEN BY";
  protected virtual string AscendingClause => "ASC";
  protected virtual string DescendingClause => "DESC";

  public static QueryBuilder From(TableId source) => new(source);

  public IQueryBuilder Select(params ColumnId[] columns)
  {
    _selections.AddRange(columns);
    return this;
  }

  public IQueryBuilder OrderBy(params OrderBy[] orderBy)
  {
    _orderBy.Clear();
    _orderBy.AddRange(orderBy);
    return this;
  }

  public IQuery Build()
  {
    StringBuilder text = new();

    if (_selections.Any())
    {
      text.Append(SelectClause).Append(' ').AppendLine(string.Join(", ", _selections.Select(Format)));
    }

    text.Append(FromClause).Append(' ').AppendLine(Format(_source, fullName: true));

    if (_orderBy.Any())
    {
      text.Append(OrderByClause).Append(' ').AppendLine(string.Join($" {ThenByClause} ", _orderBy.Select(Format)));
    }

    return new Query(text.ToString());
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

  protected virtual string Format(string identifier)
  {
    return string.Concat(IdentifierPrefix, identifier, IdentifierSuffix);
  }
}
