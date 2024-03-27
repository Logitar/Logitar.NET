using FluentValidation;

namespace Logitar.Net.Sms.Twilio.Settings;

internal class CreateMessageValidator : AbstractValidator<CreateMessageSettings>
{
  public CreateMessageValidator()
  {
    RuleFor(x => x.From).NotEmpty();
    RuleFor(x => x.To).NotEmpty();
    RuleFor(x => x.Body).NotEmpty();
  }
}
