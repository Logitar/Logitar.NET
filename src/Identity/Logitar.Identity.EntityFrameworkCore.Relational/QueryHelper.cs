using Logitar.Data;
using Logitar.Identity.Core.Payloads;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

public abstract class QueryHelper : IQueryHelper
{
  public virtual IQueryBuilder ApplyTextSearch(IQueryBuilder query, TextSearch search, params ColumnId[] columns)
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
              .Select(column => new OperatorCondition(column, IsLikeSearchTerm(term)))
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

  public abstract IQueryBuilder From(TableId table);

  protected virtual ConditionalOperator IsLikeSearchTerm(SearchTerm searchTerm)
    => Operators.IsLike(searchTerm.Value);
}
