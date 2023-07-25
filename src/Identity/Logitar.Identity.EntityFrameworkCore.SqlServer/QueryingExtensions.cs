using Logitar.Data;
using Logitar.Identity.Core.Payloads;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer;

public static class QueryingExtensions
{
  public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, SearchPayload payload)
  {
    if (payload.Skip > 0)
    {
      query = query.Skip(payload.Skip);
    }
    if (payload.Limit > 0)
    {
      query = query.Take(payload.Limit);
    }

    return query;
  }

  public static IQueryBuilder ApplyTextSearch(this IQueryBuilder query, TextSearch search, params ColumnId[] columns)
  {
    if (columns.Any())
    {
      int count = search.Terms.Count();
      if (count > 0)
      {
        List<Condition> conditions = new(capacity: count);

        foreach (SearchTerm term in search.Terms)
        {
          if (!string.IsNullOrWhiteSpace(term.Value))
          {
            conditions.Add(new OrCondition(columns
              .Select(column => new OperatorCondition(column, Operators.IsLike(term.Value)))
              .ToArray()));
          }
        }

        if (conditions.Any())
        {
          switch (search.Operator)
          {
            case SearchOperator.And:
              query = query.WhereAnd(conditions.ToArray());
              break;
            case SearchOperator.Or:
              query = query.WhereOr(conditions.ToArray());
              break;
          }
        }
      }
    }

    return query;
  }

  public static IQueryable<T> FromQuery<T>(this DbSet<T> set, IQuery query) where T : class
  {
    return set.FromSqlRaw(query.Text, query.Parameters.ToArray());
  }
}
