using Logitar.EventSourcing;
using Logitar.Identity.Core.Users;

namespace Logitar.Identity.Core;

[Trait(Traits.Category, Categories.Unit)]
public class UniqueNameAlreadyUsedExceptionTests
{
  [Theory(DisplayName = "It should construct the correct UniqueNameAlreadyUsedException.")]
  [InlineData("b1c77816-bb5a-4cb1-9001-34ad3ef879df", "admin", "UniqueName")]
  public void It_should_construct_the_correct_UniqueNameAlreadyUsedException(string? tenantId, string uniqueName, string propertyName)
  {
    var exception = new UniqueNameAlreadyUsedException<UserAggregate>(tenantId, uniqueName, propertyName);
    Assert.Equal(typeof(UserAggregate).GetName(), exception.Type);
    Assert.Equal(tenantId, exception.TenantId);
    Assert.Equal(uniqueName, exception.UniqueName);
    Assert.Equal(propertyName, exception.PropertyName);
  }

  [Fact(DisplayName = "It should throw ArgumentException when type is not a subclass of AggregateRoot.")]
  public void It_should_throw_ArgumentException_when_type_is_not_a_subclass_of_AggregateRoot()
  {
    var exception = Assert.Throws<ArgumentException>(() => new UniqueNameAlreadyUsedException(typeof(string), null, string.Empty, string.Empty));
    Assert.Equal("type", exception.ParamName);
  }
}
