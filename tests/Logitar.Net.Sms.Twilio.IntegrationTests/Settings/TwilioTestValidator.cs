using FluentValidation;

namespace Logitar.Net.Sms.Twilio.Settings;

internal class TwilioTestValidator : AbstractValidator<TwilioTestSettings>
{
  public TwilioTestValidator()
  {
    RuleFor(x => x.AccountSid).NotEmpty();
    RuleFor(x => x.AuthenticationToken).NotEmpty();

    RuleFor(x => x.CreateMessage).SetValidator(new CreateMessageValidator());
  }
}
