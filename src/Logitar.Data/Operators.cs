namespace Logitar.Data;

/// <summary>
/// TODO(fpion): document
/// </summary>
public static class Operators
{
  public static BetweenOperator IsBetween(object minValue, object maxValue) => new(minValue, maxValue);
  public static BetweenOperator IsNotBetween(object minValue, object maxValue) => new(minValue, maxValue, notBetween: true);

  public static ComparisonOperator IsEqualTo(object value) => new("=", value);
  public static ComparisonOperator IsGreaterThan(object value) => new(">", value);
  public static ComparisonOperator IsGreaterThanOrEqualTo(object value) => new(">=", value);
  public static ComparisonOperator IsLessThan(object value) => new("<", value);
  public static ComparisonOperator IsLessThanOrEqualTo(object value) => new("<=", value);
  public static ComparisonOperator IsNotEqualTo(object value) => new("<>", value);

  public static InOperator IsIn(params object[] values) => new(values);
  public static InOperator IsNotIn(params object[] values) => new(notIn: true, values);

  public static LikeOperator IsLike(string pattern) => new(pattern);
  public static LikeOperator IsNotLike(string pattern) => new(pattern, notLike: true);

  public static NullOperator IsNull() => new();
  public static NullOperator IsNotNull() => new(notNull: true);
}
