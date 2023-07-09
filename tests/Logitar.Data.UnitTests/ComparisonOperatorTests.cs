namespace Logitar.Data;

[Trait(Traits.Category, Categories.Unit)]
public class ComparisonOperatorTests
{
  [Theory(DisplayName = "Ctor: it constructs the correct ComparisonOperator.")]
  [InlineData("=", 0)]
  [InlineData(">", 0)]
  public void Ctor_it_constructs_the_correct_ComparisonOperator(string @operator, object value)
  {
    ComparisonOperator comparison = new(@operator, value);
    Assert.Equal(@operator, comparison.Operator);
    Assert.Equal(value, comparison.Value);
  }

  [Theory(DisplayName = "Ctor: it throws ArgumentException when operator is null, empty, or only white space.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void Ctor_it_throws_ArgumentException_when_operator_is_null_empty_or_only_white_space(string @operator)
  {
    var exception = Assert.Throws<ArgumentException>(() => new ComparisonOperator(@operator, 0));
    Assert.Equal("operator", exception.ParamName);
  }
}
