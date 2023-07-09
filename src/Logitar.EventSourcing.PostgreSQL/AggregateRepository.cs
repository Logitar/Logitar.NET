using Logitar.Data;
using Logitar.Data.PostgreSQL;
using Logitar.EventSourcing.Infrastructure;
using System.Data.Common;

namespace Logitar.EventSourcing.PostgreSQL;

public class AggregateRepository : Relational.AggregateRepository
{
  public AggregateRepository(DbConnection connection, IEventBus eventBus, IEventSerializer eventSerializer)
    : base(connection, eventBus, eventSerializer)
  {
  }

  protected override IQueryBuilder From(TableId source) => new PostgresQueryBuilder(source);
}
