namespace Logitar.Data.PostgreSQL;

[Trait(Traits.Category, Categories.Unit)]
public class PostgresOperatorsTests
{
  [Theory(DisplayName = "IsLikeInsensitive: it constructs the correct operator.")]
  [InlineData("%test%")]
  public void IsLikeInsensitive_it_constructs_the_correct_operator(string pattern)
  {
    InsensitiveLikeOperator like = PostgresOperators.IsLikeInsensitive(pattern);
    Assert.Equal(pattern, like.Pattern);
    Assert.False(like.NotLike);
  }

  [Theory(DisplayName = "IsNotLikeInsensitive: it constructs the correct operator.")]
  [InlineData("%test%")]
  public void IsNotLikeInsensitive_it_constructs_the_correct_operator(string pattern)
  {
    InsensitiveLikeOperator like = PostgresOperators.IsNotLikeInsensitive(pattern);
    Assert.Equal(pattern, like.Pattern);
    Assert.True(like.NotLike);
  }
}
