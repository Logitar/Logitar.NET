namespace Logitar.Data;

/// <summary>
/// TODO(fpion): document
/// </summary>
public interface IQueryBuilder
{
  IQueryBuilder Select(params ColumnId[] columns);

  IQuery Build();
}
