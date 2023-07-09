namespace Logitar.Data;

/// <summary>
/// TODO(fpion): document
/// </summary>
public record NullOperator : ConditionalOperator
{
  public NullOperator(bool notNull = false)
  {
    NotNull = notNull;
  }

  public bool NotNull { get; }
}
