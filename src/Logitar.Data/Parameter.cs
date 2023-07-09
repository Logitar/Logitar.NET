namespace Logitar.Data;

/// <summary>
/// Represents a data query parameter.
/// </summary>
public record Parameter : IParameter
{
  /// <summary>
  /// Initializes a new instance of the <see cref="Parameter"/> class.
  /// </summary>
  /// <param name="name">The name of the parameter.</param>
  /// <param name="value">The value of the parameter.</param>
  /// <exception cref="ArgumentException">The parameter name was missing.</exception>
  public Parameter(string name, object value)
  {
    if (string.IsNullOrWhiteSpace(name))
    {
      throw new ArgumentException("The parameter name is required.", nameof(name));
    }

    Name = name.Trim();
    Value = value;
  }

  /// <summary>
  /// Gets the name of the parameter.
  /// </summary>
  public string Name { get; }
  /// <summary>
  /// Gets the value of the parameter.
  /// </summary>
  public object Value { get; }
}
