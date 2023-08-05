﻿using Microsoft.Data.SqlClient;

namespace Logitar.Data.SqlServer;

/// <summary>
/// Represents the implementation of the SQL update command builder for Microsoft SQL Server.
/// </summary>
public class SqlServerUpdateBuilder : UpdateBuilder
{
  /// <summary>
  /// Initializes a new instance of the <see cref="SqlServerUpdateBuilder"/> class.
  /// </summary>
  /// <param name="source">The source table.</param>
  /// <exception cref="ArgumentException">The source table name has not been specified.</exception>
  public SqlServerUpdateBuilder(TableId source) : base(source)
  {
  }

  /// <summary>
  /// Gets the default schema of the SQL Server dialect.
  /// </summary>
  protected override string? DefaultSchema => "dbo";
  /// <summary>
  /// Gets the prefix of identifiers in the SQL Server dialect.
  /// </summary>
  protected override string? IdentifierPrefix => "[";
  /// <summary>
  /// Gets the suffix of identifiers in the SQL Server dialect.
  /// </summary>
  protected override string? IdentifierSuffix => "]";

  /// <summary>
  /// Initializes a new instance of the <see cref="SqlServerUpdateBuilder"/> class.
  /// </summary>
  /// <param name="source">The source table.</param>
  /// <returns>The SQL Server update builder.</returns>
  /// <exception cref="ArgumentException">The source table name has not been specified.</exception>
  public static SqlServerUpdateBuilder From(TableId source) => new(source);

  /// <summary>
  /// Creates a new SQL Server command parameter.
  /// </summary>
  /// <param name="parameter">The parameter information.</param>
  /// <returns>The SQL Server parameter.</returns>
  protected override object CreateParameter(IParameter parameter)
  {
    return new SqlParameter(parameter.Name, parameter.Value);
  }
}
