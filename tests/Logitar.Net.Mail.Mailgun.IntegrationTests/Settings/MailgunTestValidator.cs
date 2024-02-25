using FluentValidation;

namespace Logitar.Net.Mail.Mailgun.Settings;

internal class MailgunTestValidator : AbstractValidator<MailgunTestSettings>
{
  public MailgunTestValidator()
  {
    RuleFor(x => x.ApiKey).NotEmpty();
    RuleFor(x => x.DomainName).NotEmpty();

    RuleFor(x => x.From).SetValidator(new TestEmailValidator());

    RuleFor(x => x.To).SetValidator(new TestEmailValidator());
    RuleForEach(x => x.CC).SetValidator(new TestEmailValidator());
    RuleForEach(x => x.Bcc).SetValidator(new TestEmailValidator());
  }
}
