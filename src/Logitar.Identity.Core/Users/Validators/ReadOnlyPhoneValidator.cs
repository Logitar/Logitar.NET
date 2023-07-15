using FluentValidation;
using Logitar.Identity.Core.Users.Contact;

namespace Logitar.Identity.Core.Users.Validators;

/// <summary>
/// The validator used to validate instances of <see cref="ReadOnlyPhone"/>.
/// </summary>
public class ReadOnlyPhoneValidator : AbstractValidator<ReadOnlyPhone>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ReadOnlyPhoneValidator"/> class.
  /// </summary>
  public ReadOnlyPhoneValidator(string? propertyName = null)
  {
    RuleFor(x => x.CountryCode).NullOrNotEmpty()
      .MaximumLength(2);

    RuleFor(x => x.Number).NotEmpty()
      .MaximumLength(20);

    RuleFor(x => x.Extension).NullOrNotEmpty()
      .MaximumLength(10);

    IRuleBuilderOptions<ReadOnlyPhone, IPhoneNumber?> options = RuleFor(x => x).PhoneNumber();
    if (propertyName != null)
    {
      options = options.WithName(propertyName);
    }
  }
}
