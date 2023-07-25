using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Core.Users.Models;

public record Email : Contact, IEmailAddress
{
  public string Address { get; set; } = string.Empty;
}
