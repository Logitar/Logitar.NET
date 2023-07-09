namespace Logitar.Data;

/// <summary>
/// Represents a data query parameter.
/// </summary>
public interface IParameter
{
  /// <summary>
  /// Gets the name of the parameter.
  /// </summary>
  string Name { get; }
  /// <summary>
  /// Gets the value of the parameter.
  /// </summary>
  object Value { get; }
}
