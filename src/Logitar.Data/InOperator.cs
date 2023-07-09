namespace Logitar.Data;

/// <summary>
/// TODO(fpion): document
/// </summary>
public record InOperator : ConditionalOperator
{
  public InOperator(params object[] values) : this(notIn: false, values)
  {
  }
  public InOperator(bool notIn, params object[] values)
  {
    if (!values.Any())
    {
      throw new ArgumentException("At least one value must be specified.", nameof(values));
    }

    Values = values;
    NotIn = notIn;
  }

  public IEnumerable<object> Values { get; }
  public bool NotIn { get; }
}
