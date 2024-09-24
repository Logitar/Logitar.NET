namespace Logitar.Data;

/// <summary>
/// Represents a SQL dialect. It defines properties and methods to express SQL queries in a specific dialect.
/// </summary>
public record Dialect
{
  /// <summary>
  /// Gets the default schema of the generic dialect.
  /// </summary>
  public virtual string? DefaultSchema => null;
  /// <summary>
  /// Gets the prefix of identifiers in the generic dialect.
  /// </summary>
  public virtual string? IdentifierPrefix => null;
  /// <summary>
  /// Gets the suffix of identifiers in the generic dialect.
  /// </summary>
  public virtual string? IdentifierSuffix => null;
  /// <summary>
  /// Gets the identifier separator in the generic dialect.
  /// </summary>
  public virtual string IdentifierSeparator => ".";
  /// <summary>
  /// Gets the prefix of parameters in the generic dialect.
  /// </summary>
  public virtual string? ParameterPrefix => "@";
  /// <summary>
  /// Gets the suffix of parameters in the generic dialect.
  /// </summary>
  public virtual string? ParameterSuffix => null;

  /// <summary>
  /// Gets the SELECT clause in the generic dialect.
  /// </summary>
  public virtual string SelectClause => "SELECT";
  /// <summary>
  /// Gets the all-columns (*) clause in the generic dialect.
  /// </summary>
  public virtual string AllColumnsClause => "*";
  /// <summary>
  /// Gets the AS clause in the generic dialect.
  /// </summary>
  public virtual string AsClause => "AS";
  /// <summary>
  /// Gets the DELETE FROM clause in the generic dialect.
  /// </summary>
  public virtual string DeleteFromClause => "DELETE FROM";
  /// <summary>
  /// Gets the INSERT INTO clause in the generic dialect.
  /// </summary>
  public virtual string InsertIntoClause => "INSERT INTO";
  /// <summary>
  /// Gets the VALUES clause in the generic dialect.
  /// </summary>
  public virtual string ValuesClause => "VALUES";
  /// <summary>
  /// Gets the UPDATE clause in the generic dialect.
  /// </summary>
  public virtual string UpdateClause => "UPDATE";
  /// <summary>
  /// Gets the SET clause in the generic dialect.
  /// </summary>
  public virtual string SetClause => "SET";

  /// <summary>
  /// Gets the FROM clause in the generic dialect.
  /// </summary>
  public virtual string FromClause => "FROM";

  /// <summary>
  /// Gets the join clauses of the current dialect.
  /// </summary>
  public virtual Dictionary<JoinKind, string> JoinClauses { get; } = new();
  /// <summary>
  /// Gets the ON clause in the generic dialect.
  /// </summary>
  public virtual string OnClause => "ON";

  /// <summary>
  /// Gets the WHERE clause in the generic dialect.
  /// </summary>
  public virtual string WhereClause => "WHERE";
  /// <summary>
  /// Gets the IS clause in the generic dialect.
  /// </summary>
  public virtual string IsClause => "IS";
  /// <summary>
  /// Gets the NOT clause in the generic dialect.
  /// </summary>
  public virtual string NotClause => "NOT";
  /// <summary>
  /// Gets the BETWEEN clause in the generic dialect.
  /// </summary>
  public virtual string BetweenClause => "BETWEEN";
  /// <summary>
  /// Gets the IN clause in the generic dialect.
  /// </summary>
  public virtual string InClause => "IN";
  /// <summary>
  /// Gets the LIKE clause in the generic dialect.
  /// </summary>
  public virtual string LikeClause => "LIKE";
  /// <summary>
  /// Gets the NULL clause in the generic dialect.
  /// </summary>
  public virtual string NullClause => "NULL";
  /// <summary>
  /// Gets the comparison operators of the current dialect.
  /// </summary>
  public virtual Dictionary<string, string> ComparisonOperators { get; } = new();
  /// <summary>
  /// Gets the group operators of the current dialect.
  /// </summary>
  public virtual Dictionary<string, string> GroupOperators { get; } = new();

  /// <summary>
  /// Gets the ORDER BY clause in the generic dialect.
  /// </summary>
  public virtual string OrderByClause => "ORDER BY";
  /// <summary>
  /// Gets the ASC clause in the generic dialect.
  /// </summary>
  public virtual string AscendingClause => "ASC";
  /// <summary>
  /// Gets the DESC clause in the generic dialect.
  /// </summary>
  public virtual string DescendingClause => "DESC";

  /// <summary>
  /// Creates a new implementation-specific query parameter.
  /// </summary>
  /// <param name="parameter">The parameter information.</param>
  /// <returns>The implementation-specific parameter.</returns>
  public virtual object CreateParameter(IParameter parameter) => parameter;
}
