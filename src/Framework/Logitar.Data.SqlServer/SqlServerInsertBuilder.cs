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
  /// Gets or sets the dialect used to format to Microsoft SQL Server.
  /// </summary>
  public override Dialect Dialect { get; set; } = new SqlServerDialect();

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
