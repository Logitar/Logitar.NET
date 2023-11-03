﻿namespace Logitar.Data.PostgreSQL;

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
  /// Gets or sets the dialect used to format to SQL.
  /// </summary>
  public override Dialect Dialect { get; set; } = new PostgresDialect();

  /// <summary>
  /// Initializes a new instance of the <see cref="PostgresInsertBuilder"/> class.
  /// </summary>
  /// <param name="columns">The columns to insert into.</param>
  /// <returns>The Postgres insert builder.</returns>
  /// <exception cref="ArgumentException">No column has been specified, or a column name or table has not been specified, or multiple tables have been specified.</exception>
  public static PostgresInsertBuilder Into(params ColumnId[] columns) => new(columns);
}
