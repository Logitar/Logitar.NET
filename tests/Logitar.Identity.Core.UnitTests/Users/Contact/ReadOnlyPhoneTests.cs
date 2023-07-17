namespace Logitar.Identity.Core.Users.Contact;

[Trait(Traits.Category, Categories.Unit)]
public class ReadOnlyPhoneTests
{
  [Theory(DisplayName = "It constructs the correct ReadOnlyPhone.")]
  [InlineData("5143947377", "", "  ", false)]
  [InlineData("  5143947377  ", "   CA ", " 862   ", true)]
  public void It_constructs_the_correct_ReadOnlyPhone(string number, string? countryCode, string? extension, bool isVerified)
  {
    ReadOnlyPhone phone = new(number, countryCode, extension, isVerified);
    Assert.Equal(number.Trim(), phone.Number);
    Assert.Equal(countryCode?.CleanTrim(), phone.CountryCode);
    Assert.Equal(extension?.CleanTrim(), phone.Extension);
    Assert.Equal(isVerified, phone.IsVerified);
  }
}
