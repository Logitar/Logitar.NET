namespace Logitar.Data;

public static class QueryBuilderExtensions
{
  public static IQueryBuilder SelectAll(this IQueryBuilder builder)
  {
    return builder.Select(ColumnId.All());
  }
  public static IQueryBuilder SelectAll(this IQueryBuilder builder, TableId table)
  {
    return builder.Select(ColumnId.All(table));
  }

  public static IQueryBuilder Where(this IQueryBuilder builder, ColumnId column, ConditionalOperator @operator)
  {
    return builder.Where(new OperatorCondition(column, @operator));
  }
  public static IQueryBuilder WhereAnd(this IQueryBuilder builder, params Condition[] conditions)
  {
    return builder.Where(new AndCondition(conditions));
  }
  public static IQueryBuilder WhereOr(this IQueryBuilder builder, params Condition[] conditions)
  {
    return builder.Where(new OrCondition(conditions));
  }

  public static IQueryBuilder OrderBy(this IQueryBuilder builder, ColumnId column, bool isDescending = false)
  {
    return builder.OrderBy(new OrderBy(column, isDescending));
  }
}
