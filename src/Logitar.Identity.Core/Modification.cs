namespace Logitar.Identity.Core;

/// <summary>
/// This structure is used to track changes on a property. Its default value indicates no change.
/// Constructing it by specifying it a value indicates a change.
/// <br />It can be used to imitate a three-state object, like in JavaScript and TypeScript. When
/// IsModified is false, it can be equivalent to <i>undefined</i>. When IsModified is true, it can
/// be equivalent to <i>null</i> if its value is null, or it can have a value.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct Modification<T>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="Modification{T}"/> struct.
  /// </summary>
  /// <param name="value">The value of the modification.</param>
  public Modification(T? value)
  {
    IsModified = true;
    Value = value;
  }

  /// <summary>
  /// Gets a value indicating whether or not a modification has been applied.
  /// </summary>
  public bool IsModified { get; }
  /// <summary>
  /// Gets the value of the modification.
  /// </summary>
  public T? Value { get; }
}
