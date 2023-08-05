using Npgsql;

namespace Logitar.Data.PostgreSQL;

/// <summary>
/// Represents the implementation of the SQL insert command builder for PostgreSQL.
/// </summary>
public class PostgresInsertBuilder : InsertBuilder
{
  /// <summary>
  /// Initializes a new instance of the <see cref="PostgresInsertBuilder"/> class.
  /// </summary>
  /// <param name="columns">The columns to insert into.</param>
  /// <exception cref="ArgumentException">No column has been specified, or a column name or table has not been specified, or multiple tables have been specified.</exception>
  public PostgresInsertBuilder(params ColumnId[] columns) : base(columns)
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
  /// Initializes a new instance of the <see cref="PostgresInsertBuilder"/> class.
  /// </summary>
  /// <param name="columns">The columns to insert into.</param>
  /// <returns>The Postgres insert builder.</returns>
  /// <exception cref="ArgumentException">No column has been specified, or a column name or table has not been specified, or multiple tables have been specified.</exception>
  public static PostgresInsertBuilder Into(params ColumnId[] columns) => new(columns);

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
