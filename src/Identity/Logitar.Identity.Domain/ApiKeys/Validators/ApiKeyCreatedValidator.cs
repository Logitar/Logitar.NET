using FluentValidation;
using Logitar.Identity.Domain.ApiKeys.Events;
using Logitar.Identity.Domain.Validators;

namespace Logitar.Identity.Domain.ApiKeys.Validators;

public class ApiKeyCreatedValidator : AbstractValidator<ApiKeyCreatedEvent>
{
  public ApiKeyCreatedValidator()
  {
    When(x => x.TenantId != null,
      () => RuleFor(x => x.TenantId!).SetValidator(new TenantIdValidator()));

    RuleFor(x => x.Title).SetValidator(new TitleValidator());
  }
}
