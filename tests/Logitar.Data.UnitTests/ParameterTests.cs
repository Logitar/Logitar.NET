namespace Logitar.Data;

[Trait(Traits.Category, Categories.Unit)]
public class ParameterTests
{
  [Theory(DisplayName = "Ctor: it constructs the correct parameter.")]
  [InlineData("name", "value")]
  [InlineData("  name  ", "value")]
  public void Ctor_it_constructs_the_correct_parameter(string name, object value)
  {
    Parameter parameter = new(name, value);
    Assert.Equal(name.Trim(), parameter.Name);
    Assert.Equal(value, parameter.Value);
  }

  [Theory(DisplayName = "Ctor: it throws ArgumentException when parameter name is null, empty, or only white space.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void Ctor_it_throws_ArgumentException_when_parameter_name_is_null_empty_or_only_white_space(string? name)
  {
    var exception = Assert.Throws<ArgumentException>(() => new Parameter(name!, 0));
    Assert.Equal("name", exception.ParamName);
  }
}
