using FluentValidation;

namespace Logitar.Identity.Domain.Validators;

public class CustomAttributeValidator
{
  public CustomAttributeValidator(IValidator<string>? keyValidator = null, IValidator<string>? valueValidator = null)
  {
    KeyValidator = keyValidator ?? new CustomAttributeKeyValidator();
    ValueValidator = valueValidator ?? new CustomAttributeValueValidator();
  }

  public IValidator<string> KeyValidator { get; }
  public IValidator<string> ValueValidator { get; }

  public void ValidateAndThrow(string key, string value)
  {
    KeyValidator.ValidateAndThrow(key);
    ValueValidator.ValidateAndThrow(value);
  }
}
