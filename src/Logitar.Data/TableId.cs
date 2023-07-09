namespace Logitar.Data;

/// <summary>
/// Represents the identifier of data table.
/// </summary>
public record TableId
{
  /// <summary>
  /// The separator expected between the table name and schema.
  /// </summary>
  public const char Separator = '.';

  /// <summary>
  /// Initializes a new instance of the <see cref="TableId"/> class from an identifier, typically "{SCHEMA}.{TABLE}", and an optional alias.
  /// </summary>
  /// <param name="identifier">The identifier of the table.</param>
  /// <param name="alias">The alias of the table.</param>
  /// <exception cref="ArgumentException">The identifier is not valid, or the table name was missing.</exception>
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
  /// <summary>
  /// Initializes a new instance of the <see cref="TableId"/> class.
  /// </summary>
  /// <param name="schema">The schema of the table.</param>
  /// <param name="table">The name of the table.</param>
  /// <param name="alias">The alias of the table.</param>
  /// <exception cref="ArgumentException">The table name was missing.</exception>
  public TableId(string? schema, string table, string? alias = null) : this(alias)
  {
    if (string.IsNullOrWhiteSpace(table))
    {
      throw new ArgumentException("The table name is required.", nameof(table));
    }

    Schema = schema?.CleanTrim();
    Table = table.Trim();
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="TableId"/> class.
  /// </summary>
  /// <param name="alias">The alias of the table.</param>
  private TableId(string? alias)
  {
    Alias = alias?.CleanTrim();
  }

  /// <summary>
  /// Gets the schema of the table.
  /// </summary>
  public string? Schema { get; }
  /// <summary>
  /// Gets the name of the table.
  /// </summary>
  public string? Table { get; }
  /// <summary>
  /// Gets the alias of the table.
  /// </summary>
  public string? Alias { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="TableId"/> class only from an alias.
  /// <br />This is useful for shortening data queries by using only the table alias.
  /// </summary>
  /// <param name="alias">The alias of the table.</param>
  /// <returns></returns>
  /// <exception cref="ArgumentException">The alias of the table was missing.</exception>
  public static TableId FromAlias(string alias)
  {
    if (string.IsNullOrWhiteSpace(alias))
    {
      throw new ArgumentException("The table alias is required.", nameof(alias));
    }

    return new(alias);
  }
}
