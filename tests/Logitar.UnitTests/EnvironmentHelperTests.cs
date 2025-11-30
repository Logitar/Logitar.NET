namespace Logitar;

[Trait(Traits.Category, Categories.Unit)]
public class EnvironmentHelperTests
{
  private const string VariableName = "MyVariable";

  [Theory(DisplayName = "GetBoolean: it should return the found variable value.")]
  [InlineData(false)]
  [InlineData(true)]
  public void Given_Found_When_GetBoolean_Then_BooleanValue(bool value)
  {
    Environment.SetEnvironmentVariable(VariableName, value.ToString());
    bool variable = EnvironmentHelper.GetBoolean(VariableName);
    Assert.Equal(value, variable);
    Environment.SetEnvironmentVariable(VariableName, null);
  }

  [Theory(DisplayName = "GetBoolean: it should return the default variable value.")]
  [InlineData(false)]
  [InlineData(true)]
  public void Given_NotFound_When_GetBoolean_Then_DefaultValue(bool defaultValue)
  {
    bool variable = EnvironmentHelper.GetBoolean(VariableName, defaultValue);
    Assert.Equal(defaultValue, variable);
  }

  [Theory(DisplayName = "GetEnum: it should return the found variable value.")]
  [InlineData(DayOfWeek.Sunday)]
  public void Given_Found_When_GetEnum_Then_BooleanValue(DayOfWeek value)
  {
    Environment.SetEnvironmentVariable(VariableName, value.ToString());
    DayOfWeek variable = EnvironmentHelper.GetEnum<DayOfWeek>(VariableName);
    Assert.Equal(value, variable);
    Environment.SetEnvironmentVariable(VariableName, null);
  }

  [Theory(DisplayName = "GetEnum: it should return the default variable value.")]
  [InlineData(DayOfWeek.Friday)]
  public void Given_NotFound_When_GetEnum_Then_DefaultValue(DayOfWeek defaultValue)
  {
    DayOfWeek variable = EnvironmentHelper.GetEnum(VariableName, defaultValue);
    Assert.Equal(defaultValue, variable);
  }

  [Theory(DisplayName = "GetInt32: it should return the found variable value.")]
  [InlineData(42)]
  public void Given_Found_When_GetInt32_Then_BooleanValue(int value)
  {
    Environment.SetEnvironmentVariable(VariableName, value.ToString());
    int variable = EnvironmentHelper.GetInt32(VariableName);
    Assert.Equal(value, variable);
    Environment.SetEnvironmentVariable(VariableName, null);
  }

  [Theory(DisplayName = "GetInt32: it should return the default variable value.")]
  [InlineData(-1)]
  public void Given_NotFound_When_GetInt32_Then_DefaultValue(int defaultValue)
  {
    int variable = EnvironmentHelper.GetInt32(VariableName, defaultValue);
    Assert.Equal(defaultValue, variable);
  }

  [Theory(DisplayName = "GetString: it should return the found variable value.")]
  [InlineData(" Test   ")]
  public void Given_Found_When_GetString_Then_BooleanValue(string value)
  {
    Environment.SetEnvironmentVariable(VariableName, value);
    string variable = EnvironmentHelper.GetString(VariableName);
    Assert.Equal(value.Trim(), variable);
    Environment.SetEnvironmentVariable(VariableName, null);
  }

  [Theory(DisplayName = "GetString: it should return the default variable value.")]
  [InlineData("<>")]
  public void Given_NotFound_When_GetString_Then_DefaultValue(string defaultValue)
  {
    string variable = EnvironmentHelper.GetString(VariableName, defaultValue);
    Assert.Equal(defaultValue, variable);
  }

  [Theory(DisplayName = "GetTimeSpan: it should return the found variable value.")]
  [InlineData(" 12:34:56 ")]
  public void Given_Found_When_GetTimeSpan_Then_BooleanValue(string value)
  {
    TimeSpan timeSpan = TimeSpan.Parse(value);
    Environment.SetEnvironmentVariable(VariableName, value);
    TimeSpan variable = EnvironmentHelper.GetTimeSpan(VariableName);
    Assert.Equal(timeSpan, variable);
    Environment.SetEnvironmentVariable(VariableName, null);
  }

  [Theory(DisplayName = "GetTimeSpan: it should return the default variable value.")]
  [InlineData("00:00:00")]
  public void Given_NotFound_When_GetTimeSpan_Then_DefaultValue(string defaultValue)
  {
    TimeSpan timeSpan = TimeSpan.Parse(defaultValue);
    TimeSpan variable = EnvironmentHelper.GetTimeSpan(VariableName, timeSpan);
    Assert.Equal(timeSpan, variable);
  }

  [Theory(DisplayName = "TryGetBoolean: it should return the found variable value.")]
  [InlineData(false)]
  [InlineData(true)]
  public void Given_Found_When_TryGetBoolean_Then_BooleanValue(bool value)
  {
    Environment.SetEnvironmentVariable(VariableName, value.ToString());
    bool? variable = EnvironmentHelper.TryGetBoolean(VariableName);
    Assert.NotNull(variable);
    Assert.Equal(value, variable);
    Environment.SetEnvironmentVariable(VariableName, null);
  }

  [Fact(DisplayName = "TryGetBoolean: it should return null when the variable is not found.")]
  public void Given_NotFound_When_TryGetBoolean_Then_BooleanValue()
  {
    Assert.Null(EnvironmentHelper.TryGetBoolean(VariableName));
  }

  [Theory(DisplayName = "TryGetEnum: it should return the found variable value.")]
  [InlineData(DayOfWeek.Saturday)]
  public void Given_Found_When_TryGetEnum_Then_BooleanValue(DayOfWeek value)
  {
    Environment.SetEnvironmentVariable(VariableName, value.ToString());
    DayOfWeek? variable = EnvironmentHelper.TryGetEnum<DayOfWeek>(VariableName);
    Assert.NotNull(variable);
    Assert.Equal(value, variable);
    Environment.SetEnvironmentVariable(VariableName, null);
  }

  [Fact(DisplayName = "TryGetEnum: it should return null when the variable is not found.")]
  public void Given_NotFound_When_TryGetEnum_Then_BooleanValue()
  {
    Assert.Null(EnvironmentHelper.TryGetEnum<DayOfWeek>(VariableName));
  }

  [Theory(DisplayName = "TryGetInt32: it should return the found variable value.")]
  [InlineData(999)]
  public void Given_Found_When_TryGetInt32_Then_BooleanValue(int value)
  {
    Environment.SetEnvironmentVariable(VariableName, value.ToString());
    int? variable = EnvironmentHelper.TryGetInt32(VariableName);
    Assert.NotNull(variable);
    Assert.Equal(value, variable);
    Environment.SetEnvironmentVariable(VariableName, null);
  }

  [Fact(DisplayName = "TryGetInt32: it should return null when the variable is not found.")]
  public void Given_NotFound_When_TryGetInt32_Then_BooleanValue()
  {
    Assert.Null(EnvironmentHelper.TryGetInt32(VariableName));
  }

  [Theory(DisplayName = "TryGetString: it should return the found variable value.")]
  [InlineData("  Test  ")]
  public void Given_Found_When_TryGetString_Then_BooleanValue(string value)
  {
    Environment.SetEnvironmentVariable(VariableName, value.ToString());
    string? variable = EnvironmentHelper.TryGetString(VariableName);
    Assert.NotNull(variable);
    Assert.Equal(value.Trim(), variable);
    Environment.SetEnvironmentVariable(VariableName, null);
  }

  [Fact(DisplayName = "TryGetString: it should return null when the variable is not found.")]
  public void Given_NotFound_When_TryGetString_Then_BooleanValue()
  {
    Assert.Null(EnvironmentHelper.TryGetString(VariableName));
  }

  [Theory(DisplayName = "TryGetTimeSpan: it should return the found variable value.")]
  [InlineData(" 7.00:00:00 ")]
  public void Given_Found_When_TryGetTimeSpan_Then_BooleanValue(string value)
  {
    Environment.SetEnvironmentVariable(VariableName, value);
    TimeSpan? variable = EnvironmentHelper.TryGetTimeSpan(VariableName);
    Assert.NotNull(variable);
    Assert.Equal(TimeSpan.Parse(value.Trim()), variable);
    Environment.SetEnvironmentVariable(VariableName, null);
  }

  [Fact(DisplayName = "TryGetTimeSpan: it should return null when the variable is not found.")]
  public void Given_NotFound_When_TryGetTimeSpan_Then_BooleanValue()
  {
    Assert.Null(EnvironmentHelper.TryGetTimeSpan(VariableName));
  }
}
