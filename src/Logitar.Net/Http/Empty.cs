namespace Logitar.Net.Http;

/// <summary>
/// Represents a void type, since <see cref="Void"/> is not a valid return type in C#.
/// </summary>
public readonly struct Empty
{
  /// <summary>
  /// Gets an empty value.
  /// </summary>
  public static Empty Value { get; } = new();
}
