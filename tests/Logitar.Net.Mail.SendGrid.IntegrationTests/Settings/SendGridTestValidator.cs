using FluentValidation;

namespace Logitar.Net.Mail.SendGrid.Settings;

internal class SendGridTestValidator : AbstractValidator<SendGridTestSettings>
{
  public SendGridTestValidator()
  {
    RuleFor(x => x.ApiKey).NotEmpty();

    RuleFor(x => x.From).SetValidator(new TestEmailValidator());

    RuleFor(x => x.To).SetValidator(new TestEmailValidator());
    RuleForEach(x => x.CC).SetValidator(new TestEmailValidator());
    RuleForEach(x => x.Bcc).SetValidator(new TestEmailValidator());
  }
}
