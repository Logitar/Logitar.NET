using Npgsql;

namespace Logitar.Data.PostgreSQL;

/// <summary>
/// Represents the implementation of the SQL delete command builder for PostgreSQL.
/// </summary>
public class PostgresDeleteBuilder : DeleteBuilder
{
  /// <summary>
  /// Initializes a new instance of the <see cref="PostgresDeleteBuilder"/> class.
  /// </summary>
  /// <param name="source">The source table.</param>
  /// <exception cref="ArgumentException">The source table name is missing.</exception>
  public PostgresDeleteBuilder(TableId source) : base(source)
  {
  }

  /// <summary>
  /// Gets or sets the dialect used to format to SQL.
  /// </summary>
  public override Dialect Dialect { get; set; } = new PostgresDialect();

  /// <summary>
  /// Initializes a new instance of the <see cref="PostgresDeleteBuilder"/> class.
  /// </summary>
  /// <param name="source">The source table.</param>
  /// <returns>The Postgres delete builder.</returns>
  /// <exception cref="ArgumentException">The source table name is missing.</exception>
  public static PostgresDeleteBuilder From(TableId source) => new(source);

  /// <summary>
  /// Creates a new Postgres command parameter.
  /// </summary>
  /// <param name="parameter">The parameter information.</param>
  /// <returns>The Postgres parameter.</returns>
  protected override object CreateParameter(IParameter parameter)
  {
    return new NpgsqlParameter(parameter.Name, parameter.Value);
  }
}
