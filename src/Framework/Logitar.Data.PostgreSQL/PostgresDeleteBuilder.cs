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
  /// Gets the default schema of the Postgres dialect.
  /// </summary>
  protected override string? DefaultSchema => "public";
  /// <summary>
  /// Gets the prefix of identifiers in the Postgres dialect.
  /// </summary>
  protected override string? IdentifierPrefix => "\"";
  /// <summary>
  /// Gets the suffix of identifiers in the Postgres dialect.
  /// </summary>
  protected override string? IdentifierSuffix => "\"";

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
