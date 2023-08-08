using FluentValidation;
using Logitar.Identity.Domain.Settings;

namespace Logitar.Identity.Domain.Passwords.Validators;

public class PasswordValidator : AbstractValidator<string>
{
  public PasswordValidator(IPasswordSettings passwordSettings, string? propertyName = null)
  {
    IRuleBuilderOptions<string, string> options = RuleFor(x => x).NotEmpty();

    if (passwordSettings.RequiredLength > 1)
    {
      RuleFor(x => x).MinimumLength(passwordSettings.RequiredLength)
        .WithErrorCode("PasswordTooShort")
        .WithMessage($"Passwords must be at least {passwordSettings.RequiredLength} characters.");
    }

    if (passwordSettings.RequiredUniqueChars > 1)
    {
      RuleFor(x => x).Must(x => x.GroupBy(c => c).Count() >= passwordSettings.RequiredUniqueChars)
        .WithErrorCode("PasswordRequiresUniqueChars")
        .WithMessage($"Passwords must use at least {passwordSettings.RequiredUniqueChars} different characters.");
    }

    if (passwordSettings.RequireNonAlphanumeric)
    {
      RuleFor(x => x).Must(x => x.Any(c => !char.IsLetterOrDigit(c)))
        .WithErrorCode("PasswordRequiresNonAlphanumeric")
        .WithMessage("Passwords must have at least one non alphanumeric character.");
    }

    if (passwordSettings.RequireLowercase)
    {
      RuleFor(x => x).Must(x => x.Any(char.IsLower))
        .WithErrorCode("PasswordRequiresLower")
        .WithMessage("Passwords must have at least one lowercase ('a'-'z').");
    }

    if (passwordSettings.RequireUppercase)
    {
      RuleFor(x => x).Must(x => x.Any(char.IsUpper))
        .WithErrorCode("PasswordRequiresUpper")
        .WithMessage("Passwords must have at least one uppercase ('A'-'Z').");
    }

    if (passwordSettings.RequireDigit)
    {
      RuleFor(x => x).Must(x => x.Any(char.IsDigit))
        .WithErrorCode("PasswordRequiresDigit")
        .WithMessage("Passwords must have at least one digit ('0'-'9').");
    }

    if (propertyName != null)
    {
      options.WithName(propertyName);
    }
  }
}
