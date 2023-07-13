using FluentValidation;
using Logitar.Identity.Core.Users.Events;

namespace Logitar.Identity.Core.Users.Validators;

/// <summary>
/// The validator used to validate instance of <see cref="UserCreatedEvent"/>.
/// </summary>
public class UserCreatedValidator : AbstractValidator<UserCreatedEvent>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UserCreatedValidator"/> class.
  /// </summary>
  /// <param name="uniqueNameValidator">The validator used to validate unique names.</param>
  public UserCreatedValidator(IValidator<string> uniqueNameValidator)
  {
    RuleFor(x => x.TenantId).NullOrNotEmpty()
      .MaximumLength(byte.MaxValue);

    RuleFor(x => x.UniqueName).SetValidator(uniqueNameValidator);
  }
}
