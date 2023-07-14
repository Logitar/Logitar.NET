using FluentValidation;

namespace Logitar.Identity.Core.Users.Validators;

/// <summary>
/// The validator used to validate external identifiers.
/// </summary>
public class ExternalIdentifierValidator
{
  /// <summary>
  /// The lock to the singleton instance of the validator.
  /// </summary>
  private static readonly object _lock = new();
  /// <summary>
  /// The singleton instance of the validator.
  /// </summary>
  private static ExternalIdentifierValidator? _instance = null;
  /// <summary>
  /// Gets the singleton instance of the validator.
  /// </summary>
  public static ExternalIdentifierValidator Instance
  {
    get
    {
      lock (_lock)
      {
        _instance ??= new();
        return _instance;
      }
    }
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ExternalIdentifierValidator"/> class.
  /// </summary>
  /// <param name="key">The validator for external identifier keys.</param>
  /// <param name="value">The validator for external identifier values.</param>
  public ExternalIdentifierValidator(IValidator<string>? key = null, IValidator<string>? value = null)
  {
    Key = key ?? new ExternalIdentifierKeyValidator(nameof(Key));
    Value = value ?? new ExternalIdentifierValueValidator(nameof(Value));
  }

  /// <summary>
  /// Gets the validator for external identifier keys.
  /// </summary>
  public IValidator<string> Key { get; }
  /// <summary>
  /// Gets the validator for external identifier values.
  /// </summary>
  public IValidator<string> Value { get; }

  /// <summary>
  /// Validates the specified external identifiers and throws if validation fails.
  /// </summary>
  /// <param name="key">The key of the external identifier.</param>
  /// <param name="value">The value of the external identifier.</param>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public void ValidateAndThrow(string key, string value)
  {
    Key.ValidateAndThrow(key);
    Value.ValidateAndThrow(value);
  }
}
