namespace Logitar.Identity.Domain.Users;

public interface IPhoneNumber
{
  string? CountryCode { get; }
  string Number { get; }
  string? Extension { get; }
}
