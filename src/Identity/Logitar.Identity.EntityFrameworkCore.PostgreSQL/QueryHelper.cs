using Logitar.Data;
using Logitar.Data.PostgreSQL;
using Logitar.Identity.Core.Payloads;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL;

public class QueryHelper : Relational.QueryHelper
{
  public override IQueryBuilder From(TableId table) => PostgresQueryBuilder.From(table);

  protected override ConditionalOperator IsLikeSearchTerm(SearchTerm searchTerm)
    => PostgresOperators.IsLikeInsensitive(searchTerm.Value);
}
