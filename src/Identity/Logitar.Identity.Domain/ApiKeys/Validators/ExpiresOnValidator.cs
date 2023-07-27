using FluentValidation;
using Logitar.Identity.Domain.Validators;

namespace Logitar.Identity.Domain.ApiKeys.Validators;

public class ExpiresOnValidator : AbstractValidator<DateTime>
{
  public ExpiresOnValidator(string? propertyName = null)
  {
    IRuleBuilderOptions<DateTime, DateTime> options = RuleFor(x => x).Future();

    if (propertyName != null)
    {
      options = options.WithName(propertyName);
    }
  }
}
