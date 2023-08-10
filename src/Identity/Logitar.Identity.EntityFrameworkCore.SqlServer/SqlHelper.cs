using Logitar.Data;
using Logitar.Data.SqlServer;
using Logitar.Identity.EntityFrameworkCore.Relational;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer;

public class SqlHelper : ISqlHelper
{
  public IQueryBuilder QueryFrom(TableId table) => SqlServerQueryBuilder.From(table);
}
