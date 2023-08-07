using FluentValidation;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users.Events;
using Logitar.Identity.Domain.Validators;

namespace Logitar.Identity.Domain.Users.Validators;

public class UserCreatedValidator : AbstractValidator<UserCreatedEvent>
{
  public UserCreatedValidator(IUniqueNameSettings uniqueNameSettings)
  {
    When(x => x.TenantId != null, () => RuleFor(x => x.TenantId!)
      .SetValidator(new TenantIdValidator()));

    RuleFor(x => x.UniqueName).SetValidator(new UniqueNameValidator(uniqueNameSettings));
  }
}
