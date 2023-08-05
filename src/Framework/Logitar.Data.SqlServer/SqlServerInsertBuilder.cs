using Microsoft.Data.SqlClient;

namespace Logitar.Data.SqlServer;

/// <summary>
/// Represents the implementation of the SQL insert command builder for Microsoft SQL Server.
/// </summary>
public class SqlServerInsertBuilder : InsertBuilder
{
  /// <summary>
  /// Initializes a new instance of the <see cref="SqlServerInsertBuilder"/> class.
  /// </summary>
  /// <param name="columns">The columns to insert into.</param>
  /// <exception cref="ArgumentException">No column has been specified, or a column name or table has not been specified, or multiple tables have been specified.</exception>
  public SqlServerInsertBuilder(params ColumnId[] columns) : base(columns)
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
  /// Initializes a new instance of the <see cref="SqlServerInsertBuilder"/> class.
  /// </summary>
  /// <param name="columns">The columns to insert into.</param>
  /// <returns>The SQL Server insert builder.</returns>
  /// <exception cref="ArgumentException">No column has been specified, or a column name or table has not been specified, or multiple tables have been specified.</exception>
  public static SqlServerInsertBuilder Into(params ColumnId[] columns) => new(columns);

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
