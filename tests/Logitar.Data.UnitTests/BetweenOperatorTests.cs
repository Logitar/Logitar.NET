namespace Logitar.Data.UnitTests;

[Trait(Traits.Category, Categories.Unit)]
public class BetweenOperatorTests
{
  [Theory(DisplayName = "Ctor: it constructs the correct BetweenOperator.")]
  [InlineData(false, 1, 10)]
  [InlineData(true, 11, 100)]
  public void Ctor_it_constructs_the_correct_BetweenOperator(bool notBetween, object minValue, object maxValue)
  {
    BetweenOperator @operator = new(minValue, maxValue, notBetween);
    Assert.Equal(minValue, @operator.MinValue);
    Assert.Equal(maxValue, @operator.MaxValue);
    Assert.Equal(notBetween, @operator.NotBetween);
  }
}
