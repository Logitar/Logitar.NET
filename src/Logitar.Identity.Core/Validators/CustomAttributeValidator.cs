using FluentValidation;

namespace Logitar.Identity.Core.Validators;

/// <summary>
/// The validator used to validate custom attributes.
/// </summary>
public class CustomAttributeValidator
{
  /// <summary>
  /// The lock to the singleton instance of the validator.
  /// </summary>
  private static readonly object _lock = new();
  /// <summary>
  /// The singleton instance of the validator.
  /// </summary>
  private static CustomAttributeValidator? _instance = null;
  /// <summary>
  /// Gets the singleton instance of the validator.
  /// </summary>
  public static CustomAttributeValidator Instance
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
  /// Initializes a new instance of the <see cref="CustomAttributeValidator"/> class.
  /// </summary>
  /// <param name="key">The validator for custom attribute keys.</param>
  /// <param name="value">The validator for custom attribute values.</param>
  public CustomAttributeValidator(IValidator<string>? key = null, IValidator<string>? value = null)
  {
    Key = key ?? new CustomAttributeKeyValidator(nameof(Key));
    Value = value ?? new CustomAttributeValueValidator(nameof(Value));
  }

  /// <summary>
  /// Gets the validator for custom attribute keys.
  /// </summary>
  public IValidator<string> Key { get; }
  /// <summary>
  /// Gets the validator for custom attribute values.
  /// </summary>
  public IValidator<string> Value { get; }

  /// <summary>
  /// Validates the specified custom attributes and throws if validation fails.
  /// </summary>
  /// <param name="key">The key of the custom attribute.</param>
  /// <param name="value">The value of the custom attribute.</param>
  /// <exception cref="ValidationException">The validation failed.</exception>
  public void ValidateAndThrow(string key, string value)
  {
    Key.ValidateAndThrow(key);
    Value.ValidateAndThrow(value);
  }
}
