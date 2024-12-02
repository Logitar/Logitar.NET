namespace Logitar.Data;

[Trait(Traits.Category, Categories.Unit)]
public class LikeOperatorTests
{
  [Theory(DisplayName = "Ctor: it constructs the correct LikeOperator.")]
  [InlineData(false, "%test%")]
  [InlineData(true, "%test%")]
  public void Ctor_it_constructs_the_correct_LikeOperator(bool notLike, string pattern)
  {
    LikeOperator @operator = new(pattern, notLike);
    Assert.Equal(pattern, @operator.Pattern);
    Assert.Equal(notLike, @operator.NotLike);
  }

  [Theory(DisplayName = "Ctor: it throws ArgumentException pattern is null, empty, or only white space.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void Ctor_it_throws_ArgumentException_when_pattern_is_null_empty_or_only_white_space(string? pattern)
  {
    var exception = Assert.Throws<ArgumentException>(() => new LikeOperator(pattern!));
    Assert.Equal("pattern", exception.ParamName);
  }
}
