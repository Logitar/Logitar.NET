namespace Logitar.Data.UnitTests;

[Trait(Traits.Category, Categories.Unit)]
public class InOperatorTests
{
  [Theory(DisplayName = "Ctor: it constructs the correct InOperator.")]
  [InlineData(false, 1, 3, 5, 7)]
  [InlineData(true, "test")]
  public void Ctor_it_constructs_the_correct_InOperator(bool notIn, params object[] values)
  {
    InOperator @operator = new(notIn, values);
    Assert.Equal(notIn, @operator.NotIn);
    Assert.True(values.SequenceEqual(@operator.Values));
  }

  [Fact(DisplayName = "Ctor: it throws ArgumentException when there are no value.")]
  public void Ctor_it_throws_ArgumentException_when_there_are_no_value()
  {
    var exception = Assert.Throws<ArgumentException>(() => new InOperator(Array.Empty<object>()));
    Assert.Equal("values", exception.ParamName);
  }
}
