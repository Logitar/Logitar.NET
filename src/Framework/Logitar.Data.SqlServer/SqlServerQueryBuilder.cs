using Microsoft.Data.SqlClient;

namespace Logitar.Data.SqlServer;

/// <summary>
/// Represents the implementation of the SQL query builder for Microsoft SQL Server.
/// </summary>
public class SqlServerQueryBuilder : QueryBuilder
{
  /// <summary>
  /// Initializes a new instance of the <see cref="SqlServerQueryBuilder"/> class.
  /// </summary>
  /// <param name="source">The source table.</param>
  /// <exception cref="ArgumentException">The source table name is missing.</exception>
  public SqlServerQueryBuilder(TableId source) : base(source)
  {
  }

  /// <summary>
  /// Gets or sets the dialect used to format to Microsoft SQL Server.
  /// </summary>
  public override Dialect Dialect { get; set; } = new SqlServerDialect();

  /// <summary>
  /// Initializes a new instance of the <see cref="SqlServerQueryBuilder"/> class.
  /// </summary>
  /// <param name="source">The source table.</param>
  /// <returns>The SQL Server query builder.</returns>
  public static SqlServerQueryBuilder From(TableId source) => new(source);

  /// <summary>
  /// Creates a new SQL Server query parameter.
  /// </summary>
  /// <param name="parameter">The parameter information.</param>
  /// <returns>The SQL Server parameter.</returns>
  protected override object CreateParameter(IParameter parameter)
  {
    return new SqlParameter(parameter.Name, parameter.Value);
  }
}
