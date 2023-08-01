using FluentValidation;

namespace Logitar.Identity.Domain.Users.Validators;

public class ExternalIdentifierValidator
{
  public ExternalIdentifierValidator(IValidator<string>? keyValidator = null, IValidator<string>? valueValidator = null)
  {
    KeyValidator = keyValidator ?? new ExternalIdentifierKeyValidator("Key");
    ValueValidator = valueValidator ?? new ExternalIdentifierValueValidator("Value");
  }

  public IValidator<string> KeyValidator { get; }
  public IValidator<string> ValueValidator { get; }

  public void ValidateAndThrow(string key, string value)
  {
    KeyValidator.ValidateAndThrow(key);
    ValueValidator.ValidateAndThrow(value);
  }
}
