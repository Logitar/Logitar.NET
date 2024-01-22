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

  [Theory(DisplayName = "It should generate the correct string from a list of characters.")]
  [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_", null)]
  [InlineData("ACDEFGHJKLMNPQRSTUVWXYZ2345679", 7)]
  public void It_should_generate_the_correct_string_from_a_list_of_characters(string characters, int? count = null)
  {
    string generated = count.HasValue ? RandomStringGenerator.GetString(characters, count.Value) : RandomStringGenerator.GetString(characters);
    Assert.Equal(count ?? (256 / 8), generated.Length);
    Assert.True(generated.All(characters.Contains));
  }

  [Fact(DisplayName = "It should throw ArgumentException when the list of characters is too long.")]
  public void It_should_throw_ArgumentException_when_the_list_of_characters_is_too_long()
  {
    string characters = RandomStringGenerator.GetString(1000);
    var exception = Assert.Throws<ArgumentException>(() => RandomStringGenerator.GetString(characters));
    Assert.StartsWith("A maximum of 256 characters must be provided.", exception.Message);
    Assert.Equal("characters", exception.ParamName);
  }
}
