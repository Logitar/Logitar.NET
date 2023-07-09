namespace Logitar.Data;

/// <summary>
/// Represents a data query.
/// </summary>
public interface IQuery
{
  /// <summary>
  /// Gets the text of the query.
  /// </summary>
  string Text { get; }
  /// <summary>
  /// Gets the parameters of the query.
  /// </summary>
  IEnumerable<object> Parameters { get; }
}
