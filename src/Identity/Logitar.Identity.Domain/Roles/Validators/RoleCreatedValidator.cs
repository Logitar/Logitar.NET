using FluentValidation;
using Logitar.Identity.Domain.Roles.Events;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Validators;

namespace Logitar.Identity.Domain.Roles.Validators;

public class RoleCreatedValidator : AbstractValidator<RoleCreatedEvent>
{
  public RoleCreatedValidator(IUniqueNameSettings uniqueNameSettings)
  {
    When(x => x.TenantId != null,
      () => RuleFor(x => x.TenantId!).SetValidator(new TenantIdValidator()));

    RuleFor(x => x.UniqueName).SetValidator(new UniqueNameValidator(uniqueNameSettings));
  }
}
