using FluentValidation;
using Logitar.Identity.Domain.Users.Validators;

namespace Logitar.Identity.Domain.Users;

public record PostalAddress : Contact, IPostalAddress
{
  public PostalAddress(string street, string locality, string country, string? region = null,
    string? postalCode = null, bool isVerified = false) : base(isVerified)
  {
    Street = street.Trim();
    Locality = locality.Trim();
    Region = region?.CleanTrim();
    PostalCode = postalCode?.CleanTrim();
    Country = country.Trim();
    new PostalAddressValidator().ValidateAndThrow(this);
  }

  public string Street { get; }
  public string Locality { get; }
  public string? Region { get; }
  public string? PostalCode { get; }
  public string Country { get; }
}
