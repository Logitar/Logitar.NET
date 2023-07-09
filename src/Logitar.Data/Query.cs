namespace Logitar.Data;

/// <summary>
/// Represents a data query.
/// </summary>
internal record Query : IQuery
{
  /// <summary>
  /// Initializes a new instance of the <see cref="Query"/> class.
  /// </summary>
  /// <param name="text">The text of the query.</param>
  /// <param name="parameters">The parameters of the query.</param>
  /// <exception cref="ArgumentException">The query text was missing.</exception>
  public Query(string text, IEnumerable<object> parameters)
  {
    if (string.IsNullOrWhiteSpace(text))
    {
      throw new ArgumentException("The query text is required.", nameof(text));
    }

    Text = text.Trim();
    Parameters = parameters;
  }

  /// <summary>
  /// Gets the text of the query.
  /// </summary>
  public string Text { get; }
  /// <summary>
  /// Gets the parameters of the query.
  /// </summary>
  public IEnumerable<object> Parameters { get; }
}
