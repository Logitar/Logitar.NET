namespace Logitar.Data;

/// <summary>
/// Provides methods that ease the creation of condition operators.
/// </summary>
public static class Operators
{
  /// <summary>
  /// Creates a new instance of the <see cref="BetweenOperator"/> class, specifying an including range.
  /// </summary>
  /// <param name="minValue">The minimum value of the range.</param>
  /// <param name="maxValue">The maximum value of the range.</param>
  /// <returns>The instance of the operator.</returns>
  public static BetweenOperator IsBetween(object minValue, object maxValue) => new(minValue, maxValue);
  /// <summary>
  /// Creates a new instance of the <see cref="BetweenOperator"/> class, specifying an excluding range.
  /// </summary>
  /// <param name="minValue">The minimum value of the range.</param>
  /// <param name="maxValue">The maximum value of the range.</param>
  /// <returns>The instance of the operator.</returns>
  public static BetweenOperator IsNotBetween(object minValue, object maxValue) => new(minValue, maxValue, notBetween: true);

  /// <summary>
  /// Creates a new instance of the <see cref="ComparisonOperator"/> class, specifying an equal to (==) comparison.
  /// </summary>
  /// <param name="value">The value of the comparison.</param>
  /// <returns>The instance of the operator.</returns>
  public static ComparisonOperator IsEqualTo(object value) => new("=", value);
  /// <summary>
  /// Creates a new instance of the <see cref="ComparisonOperator"/> class, specifying a greater than (&gt;) comparison.
  /// </summary>
  /// <param name="value">The value of the comparison.</param>
  /// <returns>The instance of the operator.</returns>
  public static ComparisonOperator IsGreaterThan(object value) => new(">", value);
  /// <summary>
  /// Creates a new instance of the <see cref="ComparisonOperator"/> class, specifying a greater than or equal to (&gt;=) comparison.
  /// </summary>
  /// <param name="value">The value of the comparison.</param>
  /// <returns>The instance of the operator.</returns>
  public static ComparisonOperator IsGreaterThanOrEqualTo(object value) => new(">=", value);
  /// <summary>
  /// Creates a new instance of the <see cref="ComparisonOperator"/> class, specifying a less than (&lt;) comparison.
  /// </summary>
  /// <param name="value">The value of the comparison.</param>
  /// <returns>The instance of the operator.</returns>
  public static ComparisonOperator IsLessThan(object value) => new("<", value);
  /// <summary>
  /// Creates a new instance of the <see cref="ComparisonOperator"/> class, specifying a less than or equal to (&lt;=) comparison.
  /// </summary>
  /// <param name="value">The value of the comparison.</param>
  /// <returns>The instance of the operator.</returns>
  public static ComparisonOperator IsLessThanOrEqualTo(object value) => new("<=", value);
  /// <summary>
  /// Creates a new instance of the <see cref="ComparisonOperator"/> class, specifying a not equal to (!=) comparison.
  /// </summary>
  /// <param name="value">The value of the comparison.</param>
  /// <returns>The instance of the operator.</returns>
  public static ComparisonOperator IsNotEqualTo(object value) => new("<>", value);

  /// <summary>
  /// Creates a new instance of the <see cref="InOperator"/> class, specifying an including range.
  /// </summary>
  /// <param name="values">The values in the range.</param>
  /// <returns>The instance of the operator.</returns>
  public static InOperator IsIn(params object[] values) => new(values);
  /// <summary>
  /// Creates a new instance of the <see cref="InOperator"/> class, specifying an excluding range.
  /// </summary>
  /// <param name="values">The values in the range.</param>
  /// <returns>The instance of the operator.</returns>
  public static InOperator IsNotIn(params object[] values) => new(notIn: true, values);

  /// <summary>
  /// Creates a new instance of the <see cref="LikeOperator"/> class, specifying an including pattern.
  /// </summary>
  /// <param name="pattern">The pattern used to match strings.</param>
  /// <returns>The instance of the operator.</returns>
  public static LikeOperator IsLike(string pattern) => new(pattern);
  /// <summary>
  /// Creates a new instance of the <see cref="LikeOperator"/> class, specifying an excluding pattern.
  /// </summary>
  /// <param name="pattern">The pattern used to match strings.</param>
  /// <returns>The instance of the operator.</returns>
  public static LikeOperator IsNotLike(string pattern) => new(pattern, notLike: true);

  /// <summary>
  /// Createa a new instance of the <see cref="NullOperator"/> class, including null results.
  /// </summary>
  /// <returns>The instance of the operator.</returns>
  public static NullOperator IsNull() => new();
  /// <summary>
  /// Createa a new instance of the <see cref="NullOperator"/> class, excluding null results.
  /// </summary>
  /// <returns>The instance of the operator.</returns>
  public static NullOperator IsNotNull() => new(notNull: true);
}
