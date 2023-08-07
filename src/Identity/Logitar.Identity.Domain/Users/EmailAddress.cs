using FluentValidation;
using Logitar.Identity.Domain.Users.Validators;

namespace Logitar.Identity.Domain.Users;

public record EmailAddress : Contact, IEmailAddress
{
  public EmailAddress(string address, bool isVerified = false) : base(isVerified)
  {
    Address = address.Trim();
    new EmailAddressValidator().ValidateAndThrow(this);
  }

  public string Address { get; }
}
