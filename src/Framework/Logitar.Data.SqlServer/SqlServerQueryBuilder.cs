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
}
