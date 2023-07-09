namespace Logitar.Data;

/// <summary>
/// Represents a data query builder. It exposes methods to configure and build a data query.
/// </summary>
public interface IQueryBuilder
{
  /// <summary>
  /// Selects the specified columns in the query results.
  /// </summary>
  /// <param name="columns">The columns to select.</param>
  /// <returns>The query builder.</returns>
  IQueryBuilder Select(params ColumnId[] columns);

  /// <summary>
  /// Applies the specified conditions to the query.
  /// </summary>
  /// <param name="conditions">The conditions to apply.</param>
  /// <returns>The query builder.</returns>
  IQueryBuilder Where(params Condition[] conditions);

  /// <summary>
  /// Applies the specified sort parameters to the query.
  /// </summary>
  /// <param name="orderBy">The sort parameters to apply.</param>
  /// <returns>The query builder.</returns>
  IQueryBuilder OrderBy(params OrderBy[] orderBy);

  /// <summary>
  /// Builds the data query.
  /// </summary>
  /// <returns>The data query.</returns>
  IQuery Build();
}
