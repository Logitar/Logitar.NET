namespace Logitar.Identity.Core.Users.Contact;

[Trait(Traits.Category, Categories.Unit)]
public class ReadOnlyEmailTests
{
  [Theory(DisplayName = "It constructs the correct ReadOnlyEmail.")]
  [InlineData("info@test.com", false)]
  [InlineData("  info@test.com  ", true)]
  public void It_constructs_the_correct_ReadOnlyEmail(string address, bool isVerified)
  {
    ReadOnlyEmail email = new(address, isVerified);
    Assert.Equal(address.Trim(), email.Address);
    Assert.Equal(isVerified, email.IsVerified);
  }
}
