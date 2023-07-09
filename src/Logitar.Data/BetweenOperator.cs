namespace Logitar.Data;

/// <summary>
/// TODO(fpion): document
/// </summary>
public record BetweenOperator : ConditionalOperator
{
  public BetweenOperator(object minValue, object maxValue, bool notBetween = false)
  {
    MinValue = minValue;
    MaxValue = maxValue;
    NotBetween = notBetween;
  }

  public object MinValue { get; }
  public object MaxValue { get; }
  public bool NotBetween { get; }
}
