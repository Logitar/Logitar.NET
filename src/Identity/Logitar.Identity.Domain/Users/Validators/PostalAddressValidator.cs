using FluentValidation;
using Logitar.Identity.Domain.Validators;

namespace Logitar.Identity.Domain.Users.Validators;

public class PostalAddressValidator : AbstractValidator<IPostalAddress>
{
  public PostalAddressValidator()
  {
    RuleFor(x => x.Street).NotEmpty()
      .MaximumLength(byte.MaxValue);

    RuleFor(x => x.Locality).NotEmpty()
      .MaximumLength(byte.MaxValue);

    RuleFor(x => x.Country).NotEmpty()
      .MaximumLength(byte.MaxValue)
      .Country();

    // TODO(fpion): Region

    // TODO(fpion): PostalCode
  }
}
