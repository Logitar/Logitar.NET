namespace Logitar.Data;

/// <summary>
/// TODO(fpion): document
/// </summary>
public record LikeOperator : ConditionalOperator
{
  public LikeOperator(string pattern, bool notLike = false)
  {
    if (string.IsNullOrWhiteSpace(pattern))
    {
      throw new ArgumentException("The pattern is required.", nameof(pattern));
    }

    Pattern = pattern;
    NotLike = notLike;
  }

  public string Pattern { get; }
  public bool NotLike { get; }
}
