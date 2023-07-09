namespace Logitar.Data;

/// <summary>
/// TODO(fpion): document
/// </summary>
public interface IQueryBuilder
{
  IQueryBuilder Select(params ColumnId[] columns);

  IQueryBuilder Where(params Condition[] conditions);

  IQueryBuilder OrderBy(params OrderBy[] orderBy);

  IQuery Build();
}
