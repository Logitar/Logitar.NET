namespace Logitar.Data;

/// <summary>
/// TODO(fpion): document
/// </summary>
internal record Query : IQuery
{
  public Query(string text, IEnumerable<object> parameters)
  {
    if (string.IsNullOrWhiteSpace(text))
    {
      throw new ArgumentException("The query text is required.", nameof(text));
    }

    Text = text.Trim();
    Parameters = parameters;
  }

  public string Text { get; }
  public IEnumerable<object> Parameters { get; }
}
