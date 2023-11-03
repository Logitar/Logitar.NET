namespace Logitar.Data;

[Trait(Traits.Category, Categories.Unit)]
public class OperatorsTests
{
  [Theory(DisplayName = "IsBetween: it constructs the correct operator.")]
  [InlineData(3, 33)]
  public void IsBetween_it_constructs_the_correct_operator(object minValue, object maxValue)
  {
    BetweenOperator between = Operators.IsBetween(minValue, maxValue);
    Assert.Equal(minValue, between.MinValue);
    Assert.Equal(maxValue, between.MaxValue);
    Assert.False(between.NotBetween);
  }

  [Theory(DisplayName = "IsEqualTo: it constructs the correct operator.")]
  [InlineData(42)]
  public void IsEqualTo_it_constructs_the_correct_operator(object value)
  {
    ComparisonOperator comparison = Operators.IsEqualTo(value);
    Assert.Equal("=", comparison.Operator);
    Assert.Equal(value, comparison.Value);
  }

  [Theory(DisplayName = "IsGreaterThan: it constructs the correct operator.")]
  [InlineData(69)]
  public void IsGreaterThan_it_constructs_the_correct_operator(object value)
  {
    ComparisonOperator comparison = Operators.IsGreaterThan(value);
    Assert.Equal(">", comparison.Operator);
    Assert.Equal(value, comparison.Value);
  }

  [Theory(DisplayName = "IsGreaterThanOrEqualTo: it constructs the correct operator.")]
  [InlineData(0)]
  public void IsGreaterThanOrEqualTo_it_constructs_the_correct_operator(object value)
  {
    ComparisonOperator comparison = Operators.IsGreaterThanOrEqualTo(value);
    Assert.Equal(">=", comparison.Operator);
    Assert.Equal(value, comparison.Value);
  }

  [Theory(DisplayName = "IsIn: it constructs the correct operator.")]
  [InlineData("Warning", "Failure", "Critical")]
  public void IsIn_it_constructs_the_correct_operator(params object[] values)
  {
    InOperator @in = Operators.IsIn(values);
    Assert.Equal(values, @in.Values);
    Assert.False(@in.NotIn);
  }

  [Theory(DisplayName = "IsLessThan: it constructs the correct operator.")]
  [InlineData(1000)]
  public void IsLessThan_it_constructs_the_correct_operator(object value)
  {
    ComparisonOperator comparison = Operators.IsLessThan(value);
    Assert.Equal("<", comparison.Operator);
    Assert.Equal(value, comparison.Value);
  }

  [Theory(DisplayName = "IsLessThanOrEqualTo: it constructs the correct operator.")]
  [InlineData(100)]
  public void IsLessThanOrEqualTo_it_constructs_the_correct_operator(object value)
  {
    ComparisonOperator comparison = Operators.IsLessThanOrEqualTo(value);
    Assert.Equal("<=", comparison.Operator);
    Assert.Equal(value, comparison.Value);
  }

  [Theory(DisplayName = "IsLike: it constructs the correct operator.")]
  [InlineData("%test%")]
  public void IsLike_it_constructs_the_correct_operator(string pattern)
  {
    LikeOperator like = Operators.IsLike(pattern);
    Assert.Equal(pattern, like.Pattern);
    Assert.False(like.NotLike);
  }

  [Theory(DisplayName = "IsNotBetween: it constructs the correct operator.")]
  [InlineData(99, 999)]
  public void IsNotBetween_it_constructs_the_correct_operator(object minValue, object maxValue)
  {
    BetweenOperator between = Operators.IsNotBetween(minValue, maxValue);
    Assert.Equal(minValue, between.MinValue);
    Assert.Equal(maxValue, between.MaxValue);
    Assert.True(between.NotBetween);
  }

  [Theory(DisplayName = "IsNotEqualTo: it constructs the correct operator.")]
  [InlineData("")]
  public void IsNotEqualTo_it_constructs_the_correct_operator(object value)
  {
    ComparisonOperator comparison = Operators.IsNotEqualTo(value);
    Assert.Equal("<>", comparison.Operator);
    Assert.Equal(value, comparison.Value);
  }

  [Theory(DisplayName = "IsNotIn: it constructs the correct operator.")]
  [InlineData("Trace", "Debug")]
  public void IsNotIn_it_constructs_the_correct_operator(params object[] values)
  {
    InOperator @in = Operators.IsNotIn(values);
    Assert.Equal(values, @in.Values);
    Assert.True(@in.NotIn);
  }

  [Theory(DisplayName = "IsNotLike: it constructs the correct operator.")]
  [InlineData("%test%")]
  public void IsNotLike_it_constructs_the_correct_operator(string pattern)
  {
    LikeOperator like = Operators.IsNotLike(pattern);
    Assert.Equal(pattern, like.Pattern);
    Assert.True(like.NotLike);
  }

  [Fact(DisplayName = "IsNotNull: it constructs the correct operator.")]
  public void IsNotNull_it_constructs_the_correct_operator()
  {
    NullOperator @null = Operators.IsNotNull();
    Assert.True(@null.NotNull);
  }

  [Fact(DisplayName = "IsNull: it constructs the correct operator.")]
  public void IsNull_it_constructs_the_correct_operator()
  {
    NullOperator @null = Operators.IsNull();
    Assert.False(@null.NotNull);
  }
}
