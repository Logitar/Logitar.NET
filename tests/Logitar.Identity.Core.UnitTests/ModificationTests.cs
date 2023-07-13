namespace Logitar.Identity.Core;

[Trait(Traits.Category, Categories.Unit)]
public class ModificationTests
{
  [Theory(DisplayName = "It should be modified when it is constructed with value.")]
  [InlineData("Test123!")]
  [InlineData(9876.54)]
  public void It_should_be_modified_when_it_is_constructed_with_value(object value)
  {
    Modification<object> modification = new(value);
    Assert.True(modification.IsModified);
    Assert.Equal(value, modification.Value);
  }

  [Fact(DisplayName = "It should not be modified when it is not constructed with value.")]
  public void It_should_not_be_modified_when_it_is_not_constructed_with_value()
  {
    Modification<object> modification = new();
    Assert.False(modification.IsModified);
    Assert.Null(modification.Value);
  }
}
