namespace Logitar.Data.PostgreSQL;

[Trait(Traits.Category, Categories.Unit)]
public class InsensitiveLikeOperatorTests
{
  [Theory(DisplayName = "Ctor: it constructs the correct InsensitiveLikeOperator.")]
  [InlineData(false, "%test%")]
  [InlineData(true, "%test%")]
  public void Ctor_it_constructs_the_correct_InsensitiveLikeOperator(bool notInsensitiveLike, string pattern)
  {
    InsensitiveLikeOperator @operator = new(pattern, notInsensitiveLike);
    Assert.Equal(pattern, @operator.Pattern);
    Assert.Equal(notInsensitiveLike, @operator.NotLike);
  }

  [Theory(DisplayName = "Ctor: it throws ArgumentException pattern is null, empty, or only white space.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void Ctor_it_throws_ArgumentException_when_pattern_is_null_empty_or_only_white_space(string? pattern)
  {
    var exception = Assert.Throws<ArgumentException>(() => new InsensitiveLikeOperator(pattern!));
    Assert.Equal("pattern", exception.ParamName);
  }
}
