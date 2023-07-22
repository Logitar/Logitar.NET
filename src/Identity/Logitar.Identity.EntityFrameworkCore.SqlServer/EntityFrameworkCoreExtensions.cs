using Logitar.Data;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer;

public static class EntityFrameworkCoreExtensions
{
  public static IQueryable<T> FromQuery<T>(this DbSet<T> set, IQuery query)
    where T : class
  {
    return set.FromSqlRaw(query.Text, query.Parameters.ToArray());
  }
}
