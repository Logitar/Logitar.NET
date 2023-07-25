namespace Logitar.Identity.Domain.Users;

public record PostalAddress : ContactInformation, IPostalAddress
{
  public PostalAddress(string street, string locality, string country, string? region = null,
    string? postalCode = null, bool isVerified = false) : base(isVerified)
  {
    Street = street.Trim();
    Locality = locality.Trim();
    Country = country.Trim();
    Region = region?.CleanTrim();
    PostalCode = postalCode?.CleanTrim();
  }

  public string Street { get; }
  public string Locality { get; }
  public string Country { get; }
  public string? Region { get; }
  public string? PostalCode { get; }

  public string Format() => PostalAddressHelper.Format(this);
}
