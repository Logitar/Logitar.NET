using FluentValidation;
using Logitar.Identity.Core.Users.Contact;

namespace Logitar.Identity.Core.Users.Validators;

/// <summary>
/// The validator used to validate instances of <see cref="ReadOnlyEmail"/>.
/// </summary>
public class ReadOnlyEmailValidator : AbstractValidator<ReadOnlyEmail>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ReadOnlyEmailValidator"/> class.
  /// </summary>
  public ReadOnlyEmailValidator()
  {
    RuleFor(x => x.Address).NotEmpty()
      .MaximumLength(byte.MaxValue)
      .EmailAddress();
  }
}
