using Logitar.Data;
using Logitar.Data.SqlServer;
using Logitar.Identity.EntityFrameworkCore.Relational;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer;

public class SqlHelper : ISqlHelper
{
  public IDeleteBuilder DeleteFrom(TableId table) => SqlServerDeleteBuilder.From(table);
  public IQueryBuilder QueryFrom(TableId table) => SqlServerQueryBuilder.From(table);
}
