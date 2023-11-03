namespace Logitar.Data;

/// <summary>
/// Represents a join in a data query.
/// </summary>
public record Join
{
  /// <summary>
  /// Initializes a new instance of the <see cref="Join"/> class.
  /// </summary>
  /// <param name="right">The right column of the join, typically left of the ON clause.</param>
  /// <param name="left">The left column of the join, typically right of the ON clause.</param>
  /// <param name="condition">The condition of the join.</param>
  public Join(ColumnId right, ColumnId left, Condition? condition = null)
    : this(JoinKind.Inner, right, left, condition)
  {
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="Join"/> class.
  /// </summary>
  /// <param name="kind">The kind of the join.</param>
  /// <param name="right">The right column of the join, typically left of the ON clause.</param>
  /// <param name="left">The left column of the join, typically right of the ON clause.</param>
  /// <param name="condition">The condition of the join.</param>
  public Join(JoinKind kind, ColumnId right, ColumnId left, Condition? condition = null)
  {
    if (right.Name == null)
    {
      throw new ArgumentException("The column name is required.", nameof(right));
    }
    else if (right.Table == null)
    {
      throw new ArgumentException("The column table is required.", nameof(right));
    }

    if (left.Name == null)
    {
      throw new ArgumentException("The column name is required.", nameof(left));
    }
    else if (left.Table == null)
    {
      throw new ArgumentException("The column table is required.", nameof(left));
    }

    Kind = kind;
    Right = right;
    Left = left;
    Condition = condition;
  }

  /// <summary>
  /// Gets the kind of the join.
  /// </summary>
  public JoinKind Kind { get; }
  /// <summary>
  /// Gets the right column of the join, typically left of the ON clause.
  /// </summary>
  public ColumnId Right { get; }
  /// <summary>
  /// Gets the left column of the join, typically right of the ON clause.
  /// </summary>
  public ColumnId Left { get; }
  /// <summary>
  /// Gets the condition of the join.
  /// </summary>
  public Condition? Condition { get; }
}
