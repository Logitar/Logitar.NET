namespace Logitar.Identity.Core.Users.Contact;

[Trait(Traits.Category, Categories.Unit)]
public class PhoneNumberExtensionsTests
{
  [Fact(DisplayName = "It should format the phone to the correct format.")]
  public void It_should_format_the_phone_to_the_correct_format()
  {
    ReadOnlyPhone phone = new("5143947377", "CA", "12345");
    Assert.Equal("+15143947377", phone.FormatToE164());
  }

  [Fact(DisplayName = "It should return false when it is not a valid phone.")]
  public void It_should_return_false_when_it_is_a_valid_phone()
  {
    ReadOnlyPhone phone = new("helloworld", "CA");
    Assert.False(phone.IsValid());
  }

  [Fact(DisplayName = "It should return true when it is a valid phone.")]
  public void It_should_return_true_when_it_is_a_valid_phone()
  {
    ReadOnlyPhone phone = new("5143947377", "CA", "12345");
    Assert.True(phone.IsValid());
  }
}
