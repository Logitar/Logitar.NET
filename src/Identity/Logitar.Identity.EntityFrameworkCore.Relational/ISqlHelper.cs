using Logitar.Data;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

public interface ISqlHelper
{
  IQueryBuilder QueryFrom(TableId table);
}
