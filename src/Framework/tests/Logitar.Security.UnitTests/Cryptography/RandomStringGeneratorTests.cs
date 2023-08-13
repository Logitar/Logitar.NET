namespace Logitar.Security.Cryptography;

[Trait(Traits.Category, Categories.Unit)]
public class RandomStringGeneratorTests
{
  [Theory(DisplayName = "It should generate the correct string.")]
  [InlineData(null)]
  [InlineData(20)]
  public void It_should_generate_the_correct_string(int? count = null)
  {
    string generated = count.HasValue ? RandomStringGenerator.GetString(count.Value) : RandomStringGenerator.GetString();
    Assert.Equal(count ?? (256 / 8), generated.Length);
    Assert.True(generated.All(c => c >= 33 || c <= 126));
  }
}
