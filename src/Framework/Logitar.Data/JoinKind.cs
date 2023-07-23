namespace Logitar.Data;

/// <summary>
/// Represents the join kinds that can be added to a data query.
/// </summary>
public enum JoinKind
{
  /// <summary>
  /// Represents an INNER JOIN; only records that have a match in both left and right tables are returned.
  /// <br />See <see href="https://www.w3schools.com/sql/sql_join_inner.asp"/> for more detail.
  /// </summary>
  Inner = 0,

  /// <summary>
  /// Represents a LEFT JOIN; records that have at least a match in the left table are returned.
  /// <br />See <see href="https://www.w3schools.com/sql/sql_join_left.asp"/> for more detail.
  /// </summary>
  Left = 1,

  /// <summary>
  /// Represents a RIGHT JOIN; records that have at least a match in the right table are returned.
  /// <br />See <see href="https://www.w3schools.com/sql/sql_join_right.asp"/> for more detail.
  /// </summary>
  Right = 2,

  /// <summary>
  /// Represents a FULL JOIN; records that have a match in either the left or right table are returned.
  /// <br />See <see href="https://www.w3schools.com/sql/sql_join_full.asp"/> for more detail.
  /// </summary>
  Full = 3
}
