using Logitar.Data;
using Logitar.Data.SqlServer;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer;

public class QueryHelper : Relational.QueryHelper
{
  public override IQueryBuilder From(TableId table) => SqlServerQueryBuilder.From(table);
}
