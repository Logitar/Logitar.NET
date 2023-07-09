﻿using Npgsql;

namespace Logitar.Data.PostgreSQL;

/// <summary>
/// Represents the implementation of the SQL query builder for PostgreSQL.
/// </summary>
public class PostgresQueryBuilder : QueryBuilder
{
  /// <summary>
  /// Initializes a new instance of the <see cref="PostgresQueryBuilder"/> class.
  /// </summary>
  /// <param name="source">The source table.</param>
  /// <exception cref="ArgumentException">The source table name is missing.</exception>
  public PostgresQueryBuilder(TableId source) : base(source)
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
  /// Initializes a new instance of the <see cref="PostgresQueryBuilder"/> class.
  /// </summary>
  /// <param name="source">The source table.</param>
  /// <returns>The Postgres query builder.</returns>
  public static PostgresQueryBuilder From(TableId source) => new(source);

  /// <summary>
  /// Creates a new Postgres query parameters.
  /// </summary>
  /// <param name="parameter">The parameter information.</param>
  /// <returns>The Postgres parameter.</returns>
  protected override object CreateParameter(IParameter parameter)
  {
    return new NpgsqlParameter(parameter.Name, parameter.Value);
  }
}
