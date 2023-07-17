using FluentValidation;
using Logitar.Identity.Core.Settings;

namespace Logitar.Identity.Core.Users.Validators;

/// <summary>
/// The validator used to validate passwords.
/// </summary>
public class PasswordValidator : AbstractValidator<string>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="PasswordValidator"/> class.
  /// </summary>
  /// <param name="passwordSettings">The settings used to validate the password.</param>
  /// <param name="propertyName">The name of the validated property.</param>
  public PasswordValidator(IPasswordSettings passwordSettings, string? propertyName = null)
  {
    IRuleBuilderOptions<string, string> options = RuleFor(x => x).NotEmpty();

    if (passwordSettings.RequiredLength > 1)
    {
      options = options.MinimumLength(passwordSettings.RequiredLength)
        .WithErrorCode("PasswordTooShort")
        .WithMessage($"Passwords must be at least {passwordSettings.RequiredLength} characters.");
    }

    if (passwordSettings.RequiredUniqueChars > 1)
    {
      options = options.Must(x => x.GroupBy(c => c).Count() >= passwordSettings.RequiredUniqueChars)
        .WithErrorCode("PasswordRequiresUniqueChars")
        .WithMessage($"Passwords must use at least {passwordSettings.RequiredUniqueChars} different characters.");
    }

    if (passwordSettings.RequireNonAlphanumeric)
    {
      options = options.Must(x => x.Any(c => !char.IsLetterOrDigit(c)))
        .WithErrorCode("PasswordRequiresNonAlphanumeric")
        .WithMessage("Passwords must have at least one non alphanumeric character.");
    }

    if (passwordSettings.RequireLowercase)
    {
      options = options.Must(x => x.Any(char.IsLower))
        .WithErrorCode("PasswordRequiresLower")
        .WithMessage("Passwords must have at least one lowercase ('a'-'z').");
    }

    if (passwordSettings.RequireUppercase)
    {
      options = options.Must(x => x.Any(char.IsUpper))
        .WithErrorCode("PasswordRequiresUpper")
        .WithMessage("Passwords must have at least one uppercase ('A'-'Z').");
    }

    if (passwordSettings.RequireDigit)
    {
      options = options.Must(x => x.Any(char.IsDigit))
        .WithErrorCode("PasswordRequiresDigit")
        .WithMessage("Passwords must have at least one digit ('0'-'9').");
    }

    if (propertyName != null)
    {
      options = options.WithName(propertyName);
    }
  }
}
