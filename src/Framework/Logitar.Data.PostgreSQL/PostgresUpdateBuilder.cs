namespace Logitar.Data.PostgreSQL;

/// <summary>
/// Represents the implementation of the SQL update command builder for PostgreSQL.
/// </summary>
public class PostgresUpdateBuilder : UpdateBuilder
{
  /// <summary>
  /// Gets or sets the dialect used to format to SQL.
  /// </summary>
  public override Dialect Dialect { get; set; } = new PostgresDialect();

  /// <summary>
  /// Gets the ILIKE clause in the Postgres dialect.
  /// </summary>
  protected virtual string InsensitiveLikeClause => "ILIKE";

  /// <summary>
  /// Formats the specified column update to SQL.
  /// </summary>
  /// <param name="update">The column update to format.</param>
  /// <returns>The formatted SQL.</returns>
  protected override string Format(Update update)
  {
    if (update.Column.Name == null)
    {
      throw new ArgumentException("The column name is required.", nameof(update));
    }

    if (!Dialect.ComparisonOperators.TryGetValue("=", out string? equalOperator))
    {
      equalOperator = "=";
    }

    string newValue = update.Value == null ? Dialect.NullClause : Format(AddParameter(update.Value));

    return string.Join(' ', Format(update.Column.Name), equalOperator, newValue);
  }

  /// <summary>
  /// Formats the specified conditional operator to SQL.
  /// </summary>
  /// <param name="operator">The operator to format.</param>
  /// <returns>The formatted SQL.</returns>
  /// <exception cref="NotSupportedException">The conditional operator type is not supported.</exception>
  protected override string Format(ConditionalOperator @operator)
  {
    return @operator switch
    {
      InsensitiveLikeOperator insensitiveLike => Format(insensitiveLike),
      _ => base.Format(@operator),
    };
  }
  /// <summary>
  /// Formats the specified ILIKE operator to SQL.
  /// </summary>
  /// <param name="insensitiveLike">The operator to format.</param>
  /// <returns>The formatted SQL.</returns>
  protected virtual string Format(InsensitiveLikeOperator insensitiveLike)
  {
    StringBuilder formatted = new();

    if (insensitiveLike.NotLike)
    {
      formatted.Append(Dialect.NotClause).Append(' ');
    }

    formatted.Append(InsensitiveLikeClause).Append(' ').Append(Format(AddParameter(insensitiveLike.Pattern)));

    return formatted.ToString();
  }
}
