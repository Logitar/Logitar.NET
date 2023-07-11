using Logitar.Data;
using Logitar.EventSourcing.Infrastructure;
using System.Data.Common;

namespace Logitar.EventSourcing.Relational;

public abstract class AggregateRepositoryTests : Infrastructure.AggregateRepositoryTests, IDisposable
{
  protected AggregateRepositoryTests()
  {
    Connection = InitializeDbConnection();
  }

  public void Dispose()
  {
    Connection.Dispose();
    GC.SuppressFinalize(this);
  }

  protected DbConnection Connection { get; }

  protected override void AssertEqual(IEventEntity expected, IEventEntity actual)
  {
    Assert.Equal(expected.Id, actual.Id);
    Assert.Equal(expected.EventType, actual.EventType);
    Assert.Equal(expected.EventData, actual.EventData);
  }

  protected override IEnumerable<IEventEntity> GetEventEntities(AggregateRoot aggregate)
  {
    string aggregateType = aggregate.GetType().GetName();
    string aggregateId = aggregate.Id.Value;

    return aggregate.Changes.Select(change => new EventEntity
    {
      Id = change.Id,
      ActorId = change.ActorId,
      OccurredOn = change.OccurredOn.ToUniversalTime(),
      Version = change.Version,
      DeleteAction = change.DeleteAction,
      AggregateType = aggregateType,
      AggregateId = aggregateId,
      EventType = change.GetType().GetName(),
      EventData = EventSerializer.Instance.Serialize(change)
    });
  }

  protected override async Task<IEnumerable<IEventEntity>> LoadEventsAsync(CancellationToken cancellationToken)
  {
    IQuery query = CreateQueryBuilder(Events.Table)
      .Select(Events.Id, Events.EventType, Events.EventData)
      .Build();

    using DbCommand command = Connection.CreateCommand();
    command.CommandText = query.Text;
    command.Parameters.AddRange(query.Parameters.ToArray());

    List<EventEntity> entities = new();

    using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
    if (reader.HasRows)
    {
      while (await reader.ReadAsync(cancellationToken))
      {
        entities.Add(new EventEntity
        {
          Id = reader.GetGuid(0),
          EventType = reader.GetString(1),
          EventData = reader.GetString(2)
        });
      }
    }

    return entities;
  }

  protected override Task SeedDatabaseAsync(IEnumerable<AggregateRoot> aggregates, CancellationToken cancellationToken)
  {
    throw new NotImplementedException(); // TODO(fpion): implement
  }

  protected abstract IQueryBuilder CreateQueryBuilder(TableId source);

  protected abstract override AggregateRepository CreateRepository(IEventBus eventBus);

  protected abstract DbConnection InitializeDbConnection();
}
