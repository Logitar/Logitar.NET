using FluentValidation;
using Logitar.Identity.Core.Roles.Events;

namespace Logitar.Identity.Core.Roles.Validators;

/// <summary>
/// The validator used to validate instance of <see cref="RoleCreatedEvent"/>.
/// </summary>
public class RoleCreatedValidator : AbstractValidator<RoleCreatedEvent>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="RoleCreatedValidator"/> class.
  /// </summary>
  /// <param name="uniqueNameValidator">The validator used to validate unique names.</param>
  public RoleCreatedValidator(IValidator<string> uniqueNameValidator)
  {
    RuleFor(x => x.TenantId).NullOrNotEmpty()
      .MaximumLength(byte.MaxValue);

    RuleFor(x => x.UniqueName).SetValidator(uniqueNameValidator);
  }
}
