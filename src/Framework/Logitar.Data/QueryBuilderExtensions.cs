namespace Logitar.Data;

/// <summary>
/// Provides extension methods for <see cref="IQueryBuilder"/> instances.
/// </summary>
public static class QueryBuilderExtensions
{
  /// <summary>
  /// Selects all the columns in the query results.
  /// </summary>
  /// <param name="builder">The query builder.</param>
  /// <returns>The query builder.</returns>
  public static IQueryBuilder SelectAll(this IQueryBuilder builder)
  {
    return builder.Select(ColumnId.All());
  }
  /// <summary>
  /// Selects all the columns from the specified table in the query results.
  /// </summary>
  /// <param name="builder">The query builder.</param>
  /// <param name="table">The table to select its columns.</param>
  /// <returns>The query builder.</returns>
  public static IQueryBuilder SelectAll(this IQueryBuilder builder, TableId table)
  {
    return builder.Select(ColumnId.All(table));
  }

  /// <summary>
  /// Applies the specified INNER JOIN to the query.
  /// </summary>
  /// <param name="builder">The query builder.</param>
  /// <param name="left">The left column of the join.</param>
  /// <param name="right">The right column of the join.</param>
  /// <param name="condition">The condition of the join.</param>
  /// <returns>The query builder.</returns>
  public static IQueryBuilder Join(this IQueryBuilder builder, ColumnId left, ColumnId right, Condition? condition = null)
  {
    return builder.Join(new Join(left, right, condition));
  }
  /// <summary>
  /// Applies the specified FULL JOIN to the query.
  /// </summary>
  /// <param name="builder">The query builder.</param>
  /// <param name="left">The left column of the join.</param>
  /// <param name="right">The right column of the join.</param>
  /// <param name="condition">The condition of the join.</param>
  /// <returns>The query builder.</returns>
  public static IQueryBuilder FullJoin(this IQueryBuilder builder, ColumnId left, ColumnId right, Condition? condition = null)
  {
    return builder.Join(new Join(JoinKind.Full, left, right, condition));
  }

  /// <summary>
  /// Applies a condition on a column in the query.
  /// </summary>
  /// <param name="builder">The query builder.</param>
  /// <param name="column">The column to apply the condition to.</param>
  /// <param name="operator">The operator to apply to the condition.</param>
  /// <returns>The query builder.</returns>
  public static IQueryBuilder Where(this IQueryBuilder builder, ColumnId column, ConditionalOperator @operator)
  {
    return builder.Where(new OperatorCondition(column, @operator));
  }
  /// <summary>
  /// Applies a group of conditions to the query, using an AND operator between each condition.
  /// </summary>
  /// <param name="builder">The query builder.</param>
  /// <param name="conditions">The conditions to apply.</param>
  /// <returns>The query builder.</returns>
  public static IQueryBuilder WhereAnd(this IQueryBuilder builder, params Condition[] conditions)
  {
    return builder.Where(new AndCondition(conditions));
  }
  /// <summary>
  /// Applies a group of conditions to the query, using an OR operator between each condition.
  /// </summary>
  /// <param name="builder">The query builder.</param>
  /// <param name="conditions">The conditions to apply.</param>
  /// <returns>The query builder.</returns>
  public static IQueryBuilder WhereOr(this IQueryBuilder builder, params Condition[] conditions)
  {
    return builder.Where(new OrCondition(conditions));
  }

  /// <summary>
  /// Applies a sort on the query.
  /// </summary>
  /// <param name="builder">The query builder.</param>
  /// <param name="column">The column to use for sorting.</param>
  /// <param name="isDescending">A value indicating whether or not the sort will be descending.</param>
  /// <returns>The query builder.</returns>
  public static IQueryBuilder OrderBy(this IQueryBuilder builder, ColumnId column, bool isDescending = false)
  {
    return builder.OrderBy(new OrderBy(column, isDescending));
  }
}
