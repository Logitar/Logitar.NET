namespace Logitar;

/// <summary>
/// Represents a builder of error messages.
/// </summary>
public class ErrorMessageBuilder : IErrorMessageBuilder
{
  private readonly List<string> _lines;

  /// <summary>
  /// Initializes a new instance of the <see cref="ErrorMessageBuilder"/> class.
  /// </summary>
  public ErrorMessageBuilder()
  {
    _lines = new();
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ErrorMessageBuilder"/> class.
  /// </summary>
  /// <param name="message">An initial error message.</param>
  public ErrorMessageBuilder(string message) : this()
  {
    _lines.Add(message);
  }

  /// <summary>
  /// Adds the specified data property to the error message.
  /// </summary>
  /// <param name="key">The key of the property.</param>
  /// <param name="value">The value of the property.</param>
  /// <param name="defaultValue">The default display value to use if the value is null.</param>
  /// <returns>The error message builder.</returns>
  public IErrorMessageBuilder AddData(object key, object? value, string? defaultValue = null)
  {
    _lines.Add($"{key}: {value ?? defaultValue}");

    return this;
  }

  /// <summary>
  /// Returns the error message.
  /// </summary>
  /// <returns>The built error message.</returns>
  public string Build() => string.Join(Environment.NewLine, _lines);
}
