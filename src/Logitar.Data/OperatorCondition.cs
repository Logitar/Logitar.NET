namespace Logitar.Data;

/// <summary>
/// TODO(fpion): document
/// </summary>
public record OperatorCondition : Condition
{
  public OperatorCondition(ColumnId column, ConditionalOperator @operator)
  {
    if (string.IsNullOrWhiteSpace(column.Name))
    {
      throw new ArgumentException("The column name is required.", nameof(column));
    }

    Column = column;
    Operator = @operator;
  }

  public ColumnId Column { get; }
  public ConditionalOperator Operator { get; }
}
