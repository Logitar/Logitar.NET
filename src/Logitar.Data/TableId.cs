namespace Logitar.Data;

/// <summary>
/// TODO(fpion): document
/// </summary>
public record TableId
{
  public const char Separator = '.';

  public TableId(string identifier, string? alias = null) : this(alias)
  {
    string[] parts = identifier.Split('.');
    if (parts.Length == 1)
    {
      Table = parts.Single().CleanTrim();
    }
    else if (parts.Length == 2)
    {
      Schema = parts[0].CleanTrim();
      Table = parts[1].CleanTrim();
    }
    else
    {
      throw new ArgumentException($"The table identifier '{identifier}' is not valid.", nameof(identifier));
    }

    if (Table == null)
    {
      throw new ArgumentException("The table name is required.", nameof(identifier));
    }
  }
  public TableId(string? schema, string table, string? alias = null) : this(alias)
  {
    if (string.IsNullOrWhiteSpace(table))
    {
      throw new ArgumentException("The table name is required.", nameof(table));
    }

    Schema = schema?.CleanTrim();
    Table = table.Trim();
  }
  private TableId(string? alias)
  {
    Alias = alias?.CleanTrim();
  }

  public string? Schema { get; }
  public string? Table { get; }
  public string? Alias { get; }

  public static TableId FromAlias(string alias)
  {
    if (string.IsNullOrWhiteSpace(alias))
    {
      throw new ArgumentException("The table alias is required.", nameof(alias));
    }

    return new(alias);
  }
}
