namespace Logitar.Data;

/// <summary>
/// TODO(fpion): document
/// </summary>
public class QueryBuilder : IQueryBuilder
{
  private readonly TableId _source;

  public QueryBuilder(TableId source)
  {
    _source = source;
  }

  protected virtual string? DefaultSchema => null;
  protected virtual string? IdentifierPrefix => null;
  protected virtual string? IdentifierSuffix => null;
  protected virtual string IdentifierSeparator => ".";

  protected virtual string FromClause => "FROM";

  public static QueryBuilder From(TableId source) => new(source);

  public IQuery Build()
  {
    StringBuilder text = new();

    text.Append(FromClause).Append(' ').AppendLine(Format(_source, fullName: true));

    return new Query(text.ToString());
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

    formatted.Append(Format(table.Table));

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
