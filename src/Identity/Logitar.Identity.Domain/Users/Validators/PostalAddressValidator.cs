using FluentValidation;

namespace Logitar.Identity.Domain.Users.Validators;

public class PostalAddressValidator : AbstractValidator<IPostalAddress>
{
  public PostalAddressValidator()
  {
    RuleFor(x => x.Street).NotEmpty()
      .MaximumLength(byte.MaxValue);

    RuleFor(x => x.Locality).NotEmpty()
      .MaximumLength(byte.MaxValue);

    When(x => PostalAddressHelper.GetSettings(x.Country)?.PostalCode != null,
      () => RuleFor(x => x.PostalCode).NotEmpty()
        .MaximumLength(byte.MaxValue)
        .Matches(x => PostalAddressHelper.GetSettings(x.Country)!.PostalCode)
    ).Otherwise(() => When(x => x.PostalCode != null, () => RuleFor(x => x.PostalCode).NotEmpty()
      .MaximumLength(byte.MaxValue))
    );

    When(x => PostalAddressHelper.GetSettings(x.Country)?.Regions != null,
      () => RuleFor(x => x.Region).NotEmpty()
        .MaximumLength(byte.MaxValue)
        .Must((x, region) => PostalAddressHelper.GetSettings(x.Country)?.Regions?.Contains(region) == true)
        .WithErrorCode("RegionValidator")
        .WithMessage(x => $"'{{PropertyName}}' must be one of the following: {string.Join(", ", PostalAddressHelper.GetSettings(x.Country)!.Regions!)}")
    ).Otherwise(() => When(x => x.Region != null, () => RuleFor(x => x.Region).NotEmpty()
      .MaximumLength(byte.MaxValue))
    );

    RuleFor(x => x.Country).NotEmpty()
      .MaximumLength(byte.MaxValue)
      .Must(PostalAddressHelper.IsSupported)
      .WithErrorCode("CountryValidator")
      .WithMessage($"'{{PropertyName}}' must be one of the following: {string.Join(", ", PostalAddressHelper.SupportedCountries)}");
  }
}
