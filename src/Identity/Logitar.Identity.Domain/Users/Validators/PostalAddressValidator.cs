using FluentValidation;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Validators;

namespace Logitar.Identity.Domain.Users.Validators;

public class PostalAddressValidator : AbstractValidator<IPostalAddress>
{
  public PostalAddressValidator(string country)
  {
    ICountrySettings? countrySettings = PostalAddressHelper.GetSettings(country);

    RuleFor(x => x.Street).NotEmpty()
      .MaximumLength(byte.MaxValue);

    RuleFor(x => x.Locality).NotEmpty()
      .MaximumLength(byte.MaxValue);

    RuleFor(x => x.Country).NotEmpty()
      .MaximumLength(byte.MaxValue)
      .Country();

    if (countrySettings?.Regions == null)
    {
      When(x => x.Region != null, () => RuleFor(x => x.Region).NotEmpty()
        .MaximumLength(byte.MaxValue));
    }
    else
    {
      RuleFor(x => x.Region).NotEmpty()
        .MaximumLength(byte.MaxValue)
        .Region(countrySettings.Regions);
    }

    if (countrySettings?.PostalCode == null)
    {
      When(x => x.PostalCode != null, () => RuleFor(x => x.PostalCode).NotEmpty()
        .MaximumLength(byte.MaxValue));
    }
    else
    {
      RuleFor(x => x.PostalCode).NotEmpty()
        .MaximumLength(byte.MaxValue)
        .PostalCode(countrySettings.PostalCode);
    }
  }
}
