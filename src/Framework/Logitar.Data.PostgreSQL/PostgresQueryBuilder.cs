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
  /// Gets the ILIKE clause in the generic dialect.
  /// </summary>
  protected virtual string InsensitiveLikeClause => "ILIKE";

  /// <summary>
  /// Initializes a new instance of the <see cref="PostgresQueryBuilder"/> class.
  /// </summary>
  /// <param name="source">The source table.</param>
  /// <returns>The Postgres query builder.</returns>
  public static PostgresQueryBuilder From(TableId source) => new(source);

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
      formatted.Append(NotClause).Append(' ');
    }

    formatted.Append(InsensitiveLikeClause).Append(' ').Append(Format(AddParameter(insensitiveLike.Pattern)));

    return formatted.ToString();
  }

  /// <summary>
  /// Creates a new Postgres query parameter.
  /// </summary>
  /// <param name="parameter">The parameter information.</param>
  /// <returns>The Postgres parameter.</returns>
  protected override object CreateParameter(IParameter parameter)
  {
    return new NpgsqlParameter(parameter.Name, parameter.Value);
  }
}
