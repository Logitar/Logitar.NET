namespace Logitar.Identity.Domain.Users;

public record EmailAddress : ContactInformation, IEmailAddress
{
  public EmailAddress(string address, bool isVerified = false) : base(isVerified)
  {
    Address = address.Trim();
  }

  public string Address { get; }
}
