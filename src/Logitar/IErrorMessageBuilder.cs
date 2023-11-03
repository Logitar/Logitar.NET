namespace Logitar;

/// <summary>
/// Describes a builder of error messages.
/// </summary>
public interface IErrorMessageBuilder
{
  /// <summary>
  /// Adds the specified data property to the error message.
  /// </summary>
  /// <param name="key">The key of the property.</param>
  /// <param name="value">The value of the property.</param>
  /// <param name="defaultValue">The default display value to use if the value is null.</param>
  /// <returns>The error message builder.</returns>
  IErrorMessageBuilder AddData(object key, object? value, string? defaultValue = null);

  /// <summary>
  /// Returns the error message.
  /// </summary>
  /// <returns>The built error message.</returns>
  string Build();
}
