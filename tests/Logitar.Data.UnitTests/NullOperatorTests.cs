namespace Logitar.Data.UnitTests;

[Trait(Traits.Category, Categories.Unit)]
public class NullOperatorTests
{
  [Theory(DisplayName = "Ctor: it constructs the correct NullOperator.")]
  [InlineData(false)]
  [InlineData(true)]
  public void Ctor_it_constructs_the_correct_NullOperator(bool notNull)
  {
    NullOperator @operator = new(notNull);
    Assert.Equal(notNull, @operator.NotNull);
  }
}
