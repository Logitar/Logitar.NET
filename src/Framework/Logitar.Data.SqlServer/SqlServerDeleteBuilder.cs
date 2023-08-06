namespace Logitar.Data.SqlServer;

/// <summary>
/// Represents the implementation of the SQL delete command builder for Microsoft SQL Server.
/// </summary>
public class SqlServerDeleteBuilder : DeleteBuilder
{
  /// <summary>
  /// Initializes a new instance of the <see cref="SqlServerDeleteBuilder"/> class.
  /// </summary>
  /// <param name="source">The source table.</param>
  /// <exception cref="ArgumentException">The source table name has not been specified.</exception>
  public SqlServerDeleteBuilder(TableId source) : base(source)
  {
  }

  /// <summary>
  /// Gets or sets the dialect used to format to Microsoft SQL Server.
  /// </summary>
  public override Dialect Dialect { get; set; } = new SqlServerDialect();

  /// <summary>
  /// Initializes a new instance of the <see cref="SqlServerDeleteBuilder"/> class.
  /// </summary>
  /// <param name="source">The source table.</param>
  /// <returns>The SQL Server delete builder.</returns>
  /// <exception cref="ArgumentException">The source table name has not been specified.</exception>
  public static SqlServerDeleteBuilder From(TableId source) => new(source);
}
