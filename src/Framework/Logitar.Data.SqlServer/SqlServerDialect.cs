using Microsoft.Data.SqlClient;

namespace Logitar.Data.SqlServer;

/// <summary>
/// Represents the dialect of Microsoft SQL Server.
/// </summary>
public record SqlServerDialect : Dialect
{
  /// <summary>
  /// Gets the default schema of the SQL Server dialect.
  /// </summary>
  public override string? DefaultSchema => "dbo";
  /// <summary>
  /// Gets the prefix of identifiers in the SQL Server dialect.
  /// </summary>
  public override string? IdentifierPrefix => "[";
  /// <summary>
  /// Gets the suffix of identifiers in the SQL Server dialect.
  /// </summary>
  public override string? IdentifierSuffix => "]";

  /// <summary>
  /// Creates a new SQL Server command parameter.
  /// </summary>
  /// <param name="parameter">The parameter information.</param>
  /// <returns>The SQL Server parameter.</returns>
  public override object CreateParameter(IParameter parameter)
  {
    return new SqlParameter(parameter.Name, parameter.Value);
  }
}
