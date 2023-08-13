using Logitar.Data;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

public static class QueryingExtensions
{
  public static IQueryable<T> FromQuery<T>(this DbSet<T> set, IQuery query) where T : class
    => set.FromSqlRaw(query.Text, query.Parameters.ToArray());
}
