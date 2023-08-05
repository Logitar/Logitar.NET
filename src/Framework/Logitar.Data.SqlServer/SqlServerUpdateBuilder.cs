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
  /// Gets or sets the dialect used to format to Microsoft SQL Server.
  /// </summary>
  public override Dialect Dialect { get; set; } = new SqlServerDialect();

  /// <summary>
  /// Initializes a new instance of the <see cref="SqlServerUpdateBuilder"/> class.
  /// </summary>
  /// <param name="source">The source table.</param>
  /// <returns>The SQL Server update builder.</returns>
  /// <exception cref="ArgumentException">The source table name has not been specified.</exception>
  public static SqlServerUpdateBuilder From(TableId source) => new(source);
}
