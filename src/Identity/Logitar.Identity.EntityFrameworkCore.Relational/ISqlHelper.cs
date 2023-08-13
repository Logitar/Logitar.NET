using Logitar.Data;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

public interface ISqlHelper
{
  IDeleteBuilder DeleteFrom(TableId table);
  IQueryBuilder QueryFrom(TableId table);
}
