namespace Logitar.Data;

/// <summary>
/// TODO(fpion): document
/// </summary>
internal record Query : IQuery
{
  public Query(string text)
  {
    Text = text;
  }

  public string Text { get; }
}
