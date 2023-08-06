namespace Logitar.Data.SqlServer;

/// <summary>
/// Represents the implementation of the SQL update command builder for Microsoft SQL Server.
/// </summary>
public class SqlServerUpdateBuilder : UpdateBuilder
{
  /// <summary>
  /// Gets or sets the dialect used to format to Microsoft SQL Server.
  /// </summary>
  public override Dialect Dialect { get; set; } = new SqlServerDialect();
}
