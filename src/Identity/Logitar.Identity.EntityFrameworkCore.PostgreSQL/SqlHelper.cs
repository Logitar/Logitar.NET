using Logitar.Data;
using Logitar.Data.PostgreSQL;
using Logitar.Identity.EntityFrameworkCore.Relational;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL;

public class SqlHelper : ISqlHelper
{
  public IDeleteBuilder DeleteFrom(TableId table) => PostgresDeleteBuilder.From(table);
  public IQueryBuilder QueryFrom(TableId table) => PostgresQueryBuilder.From(table);
}
