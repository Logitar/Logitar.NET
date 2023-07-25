using FluentValidation;
using Logitar.Identity.Domain.Validators;

namespace Logitar.Identity.Domain.Users.Validators;

public class BirthdateValidator : AbstractValidator<DateTime>
{
  public BirthdateValidator(string? propertyName = null)
  {
    IRuleBuilderOptions<DateTime, DateTime> options = RuleFor(x => x).Past();

    if (propertyName != null)
    {
      options = options.WithName(propertyName);
    }
  }
}
