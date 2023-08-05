namespace Logitar.Data.PostgreSQL;

/// <summary>
/// Represents the dialect of PostgreSQL.
/// </summary>
public record PostgresDialect : Dialect
{
  /// <summary>
  /// Gets the default schema of the Postgres dialect.
  /// </summary>
  public override string? DefaultSchema => "public";
  /// <summary>
  /// Gets the prefix of identifiers in the Postgres dialect.
  /// </summary>
  public override string? IdentifierPrefix => "\"";
  /// <summary>
  /// Gets the suffix of identifiers in the Postgres dialect.
  /// </summary>
  public override string? IdentifierSuffix => "\"";
}
