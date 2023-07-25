using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Core.Users.Models;

public record Phone : Contact, IPhoneNumber
{
  public string? CountryCode { get; set; }
  public string Number { get; set; } = string.Empty;
  public string? Extension { get; set; }
  public string E164Formatted { get; set; } = string.Empty;
}
