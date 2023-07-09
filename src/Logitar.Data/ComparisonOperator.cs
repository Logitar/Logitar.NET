namespace Logitar.Data;

/// <summary>
/// TODO(fpion): document
/// </summary>
public record ComparisonOperator : ConditionalOperator
{
  public ComparisonOperator(string @operator, object value)
  {
    if (string.IsNullOrWhiteSpace(@operator))
    {
      throw new ArgumentException("The comparison operator is required.", nameof(@operator));
    }

    Operator = @operator.Trim();
    Value = value;
  }

  public string Operator { get; }
  public object Value { get; }
}
