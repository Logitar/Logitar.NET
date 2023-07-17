namespace Logitar.Identity.Core.Users.Contact;

[Trait(Traits.Category, Categories.Unit)]
public class ReadOnlyAddressTests
{
  [Theory(DisplayName = "It constructs the correct ReadOnlyAddress.")]
  [InlineData("Bd Roméo Vachon Nord", "Dorval", "CA", "", "  ", false)]
  [InlineData("  Bd Roméo Vachon Nord  ", " Dorval   ", "   CA ", "QC", "H4Y 1H1", true)]
  public void It_constructs_the_correct_ReadOnlyAddress(string street, string locality, string country, string? region, string? postalCode, bool isVerified)
  {
    ReadOnlyAddress email = new(street, locality, country, region, postalCode, isVerified);
    Assert.Equal(street.Trim(), email.Street);
    Assert.Equal(locality.Trim(), email.Locality);
    Assert.Equal(country.Trim(), email.Country);
    Assert.Equal(region?.CleanTrim(), email.Region);
    Assert.Equal(postalCode?.CleanTrim(), email.PostalCode);
    Assert.Equal(isVerified, email.IsVerified);
  }
}
