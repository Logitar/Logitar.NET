namespace Logitar.Identity.Core.Users.Models;

public record Email : Contact
{
  public string Address { get; set; } = string.Empty;
}
