using Logitar.Data;
using Logitar.Data.PostgreSQL;
using Logitar.EventSourcing.Infrastructure;
using System.Data.Common;

namespace Logitar.EventSourcing.PostgreSQL;

/// <summary>
/// Represents the PostgreSQL implementation of an interface that allows retrieving and storing events in an event store.
/// </summary>
public class AggregateRepository : Relational.AggregateRepository
{
  /// <summary>
  /// Initializes a new instance of the <see cref="AggregateRepository"/> class.
  /// </summary>
  /// <param name="connection">The database connection.</param>
  /// <param name="eventBus">The event bus.</param>
  public AggregateRepository(DbConnection connection, IEventBus eventBus) : base(connection, eventBus)
  {
  }

  /// <summary>
  /// Returns the PostgreSQL implementation of a query builder.
  /// </summary>
  /// <param name="source">The source table.</param>
  /// <returns>The query builder.</returns>
  protected override IQueryBuilder From(TableId source) => new PostgresQueryBuilder(source);
}
