using FluentValidation;
using Logitar.Identity.Core.Settings;
using Logitar.Identity.Core.Users.Events;
using Logitar.Identity.Core.Validators;

namespace Logitar.Identity.Core.Users.Validators;

public class UserCreatedValidator : AbstractValidator<UserCreatedEvent>
{
  public UserCreatedValidator(IUniqueNameSettings uniqueNameSettings)
  {
    When(x => x.TenantId != null,
      () => RuleFor(x => x.TenantId!).SetValidator(new TenantIdValidator()));

    RuleFor(x => x.UniqueName).SetValidator(new UniqueNameValidator(uniqueNameSettings));
  }
}
