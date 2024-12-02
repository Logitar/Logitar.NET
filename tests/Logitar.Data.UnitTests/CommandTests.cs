namespace Logitar.Data;

[Trait(Traits.Category, Categories.Unit)]
public class CommandTests
{
  [Theory(DisplayName = "Ctor: it constructs the correct command.")]
  [InlineData("  DELETE FROM [MyTable];  ")]
  [InlineData("DELETE FROM [MyTable];", 2, 4, 6, 8)]
  public void Ctor_it_constructs_the_correct_command(string text, params object[] parameters)
  {
    Command command = new(text, parameters);
    Assert.Equal(text.Trim(), command.Text);
    Assert.Same(parameters, command.Parameters);
  }

  [Theory(DisplayName = "Ctor: it throws ArgumentException when text is null, empty, or only white space.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void Ctor_it_throws_ArgumentException_when_text_is_null_empty_or_only_white_space(string? text)
  {
    var exception = Assert.Throws<ArgumentException>(() => new Command(text!, Enumerable.Empty<object>()));
  }
}
