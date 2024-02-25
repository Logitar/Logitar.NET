using FluentValidation;

namespace Logitar.Net.Mail.SendGrid.Settings;

internal class TestEmailValidator : AbstractValidator<EmailSettings>
{
  public TestEmailValidator()
  {
    RuleFor(x => x.Address).NotEmpty().EmailAddress();
    When(x => x.DisplayName != null, () => RuleFor(x => x.DisplayName).NotEmpty());
  }
}
