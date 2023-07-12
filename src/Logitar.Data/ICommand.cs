namespace Logitar.Data;

/// <summary>
/// Represents a data command.
/// </summary>
public interface ICommand
{
  /// <summary>
  /// Gets the text of the command.
  /// </summary>
  string Text { get; }
  /// <summary>
  /// Gets the parameters of the command.
  /// </summary>
  IEnumerable<object> Parameters { get; }
}
