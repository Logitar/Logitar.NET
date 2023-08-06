namespace Logitar.Data;

/// <summary>
/// Represents a condition testing an operator against a column in a data query.
/// </summary>
public record OperatorCondition : Condition
{
  /// <summary>
  /// Initializes a new instance of the <see cref="OperatorCondition"/> class.
  /// </summary>
  /// <param name="column">The column against which the operator will be tested.</param>
  /// <param name="operator">The operator that will be tested against the column.</param>
  /// <exception cref="ArgumentException">The column name was missing.</exception>
  public OperatorCondition(ColumnId column, ConditionalOperator @operator)
  {
    if (string.IsNullOrWhiteSpace(column.Name))
    {
      throw new ArgumentException("The column name is required.", nameof(column));
    }

    Column = column;
    Operator = @operator;
  }

  /// <summary>
  /// Gets the column against which the operator will be tested.
  /// </summary>
  public ColumnId Column { get; }
  /// <summary>
  /// Gets the operator that will be tested against the column.
  /// </summary>
  public ConditionalOperator Operator { get; }
}
