using FluentValidation;
using Logitar.Identity.Core.Users.Contact;

namespace Logitar.Identity.Core.Users.Validators;

/// <summary>
/// The validator used to validate instances of <see cref="ReadOnlyAddress"/>.
/// </summary>
public class ReadOnlyAddressValidator : AbstractValidator<ReadOnlyAddress>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ReadOnlyAddressValidator"/> class.
  /// </summary>
  public ReadOnlyAddressValidator()
  {
    RuleFor(x => x.Street).NotEmpty()
      .MaximumLength(byte.MaxValue);

    RuleFor(x => x.Locality).NotEmpty()
      .MaximumLength(byte.MaxValue);

    RuleFor(x => x.Country).NotEmpty()
      .MaximumLength(byte.MaxValue)
      .Country();

    When(x => PostalAddressHelper.Instance.GetCountry(x.Country)?.Regions != null,
      () => RuleFor(x => x.Region).NotEmpty()
        .MaximumLength(byte.MaxValue)
        .Must((address, region) => PostalAddressHelper.Instance.GetCountry(address.Country)!.Regions!.Contains(region))
          .WithErrorCode("RegionValidator")
          .WithMessage(x => $"'{{PropertyName}}' must be one of the following: {string.Join(", ", PostalAddressHelper.Instance.GetCountry(x.Country)!.Regions!)}")
    ).Otherwise(() => RuleFor(x => x.Region).NullOrNotEmpty()
        .MaximumLength(byte.MaxValue));

    When(x => PostalAddressHelper.Instance.GetCountry(x.Country)?.PostalCode != null,
      () => RuleFor(x => x.PostalCode).NotEmpty()
        .MaximumLength(byte.MaxValue)
        .Matches(x => PostalAddressHelper.Instance.GetCountry(x.Country)!.PostalCode)
          .WithErrorCode("PostalCodeValidator")
          .WithMessage(x => $"'{{PropertyName}}' must match the following regular expression: {PostalAddressHelper.Instance.GetCountry(x.Country)!.PostalCode}")
    ).Otherwise(() => RuleFor(x => x.PostalCode).NullOrNotEmpty()
        .MaximumLength(byte.MaxValue));
  }
}
