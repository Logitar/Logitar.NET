using Logitar.Data;
using Logitar.Identity.Core.Payloads;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

public interface IQueryHelper
{
  IQueryBuilder ApplyTextSearch(IQueryBuilder queryBuilder, TextSearch search, params ColumnId[] columns);
  IQueryBuilder From(TableId table);
}
