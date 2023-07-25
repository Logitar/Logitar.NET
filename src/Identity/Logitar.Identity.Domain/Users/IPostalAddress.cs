namespace Logitar.Identity.Domain.Users;

public interface IPostalAddress
{
  string Street { get; }
  string Locality { get; }
  string Country { get; }
  string? Region { get; }
  string? PostalCode { get; }
}
