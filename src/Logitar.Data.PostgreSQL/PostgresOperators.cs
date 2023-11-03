namespace Logitar.Data.PostgreSQL;

/// <summary>
/// Provides methods that ease the creation of PostgreSQL condition operators.
/// </summary>
public static class PostgresOperators
{
  /// <summary>
  /// Creates a new instance of the <see cref="InsensitiveLikeOperator"/> class, specifying an including pattern.
  /// </summary>
  /// <param name="pattern">The pattern used to match strings.</param>
  /// <returns>The instance of the operator.</returns>
  public static InsensitiveLikeOperator IsLikeInsensitive(string pattern) => new(pattern);
  /// <summary>
  /// Creates a new instance of the <see cref="InsensitiveLikeOperator"/> class, specifying an excluding pattern.
  /// </summary>
  /// <param name="pattern">The pattern used to match strings.</param>
  /// <returns>The instance of the operator.</returns>
  public static InsensitiveLikeOperator IsNotLikeInsensitive(string pattern) => new(pattern, notLike: true);
}
