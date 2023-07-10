using Logitar.Data;
using Logitar.EventSourcing.Infrastructure;
using System.Data;
using System.Data.Common;

namespace Logitar.EventSourcing.Relational;

public abstract class AggregateRepository : Infrastructure.AggregateRepository
{
  public AggregateRepository(DbConnection connection, IEventBus eventBus) : base(eventBus)
  {
    Connection = connection;
  }

  protected DbConnection Connection { get; }

  protected override async Task<IEnumerable<DomainEvent>> LoadChangesAsync<T>(AggregateId id, long? version, CancellationToken cancellationToken)
  {
    string aggregateType = typeof(T).GetName();
    string aggregateId = id.Value;

    List<Condition> conditions = new(capacity: 3)
    {
      new OperatorCondition(Events.AggregateType, Operators.IsEqualTo(aggregateType)),
      new OperatorCondition(Events.AggregateId, Operators.IsEqualTo(aggregateId))
    };
    if (version.HasValue)
    {
      conditions.Add(new OperatorCondition(Events.Version, Operators.IsLessThanOrEqualTo(version.Value)));
    }

    IQuery query = From(Events.Table)
      .WhereAnd(conditions.ToArray())
      .OrderBy(Events.Version)
      .Select(Events.Id, Events.EventType, Events.EventData)
      .Build();

    return await ReadChangesAsync(query, cancellationToken);
  }

  protected override async Task<IEnumerable<DomainEvent>> LoadChangesAsync<T>(CancellationToken cancellationToken)
  {
    string aggregateType = typeof(T).GetName();

    IQuery query = From(Events.Table)
      .Where(Events.AggregateId, Operators.IsEqualTo(aggregateType))
      .OrderBy(Events.Version)
      .Select(Events.Id, Events.EventType, Events.EventData)
      .Build();

    return await ReadChangesAsync(query, cancellationToken);
  }

  protected override async Task<IEnumerable<DomainEvent>> LoadChangesAsync<T>(IEnumerable<AggregateId> ids, CancellationToken cancellationToken)
  {
    string aggregateType = typeof(T).GetName();
    string[] aggregateIds = ids.Select(id => id.Value).Distinct().ToArray();

    IQuery query = From(Events.Table)
      .WhereAnd(new OperatorCondition(Events.AggregateType, Operators.IsEqualTo(aggregateType)),
        new OperatorCondition(Events.AggregateId, Operators.IsIn(aggregateIds)))
      .OrderBy(Events.Version)
      .Select(Events.Id, Events.EventType, Events.EventData)
      .Build();

    return await ReadChangesAsync(query, cancellationToken);
  }

  protected virtual async Task<IEnumerable<DomainEvent>> ReadChangesAsync(IQuery query, CancellationToken cancellationToken)
  {
    using DbCommand command = Connection.CreateCommand();
    command.CommandText = query.Text;
    foreach (object parameter in query.Parameters)
    {
      command.Parameters.Add(parameter);
    }

    List<DomainEvent> changes = new();

    using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
    if (reader.HasRows)
    {
      while (await reader.ReadAsync(cancellationToken))
      {
        EventEntity entity = new()
        {
          Id = reader.GetGuid(0),
          EventType = reader.GetString(1),
          EventData = reader.GetString(2)
        };

        changes.Add(EventSerializer.Instance.Deserialize(entity));
      }
    }

    return changes.AsReadOnly();
  }

  protected override Task SaveChangesAsync(IEnumerable<AggregateRoot> aggregates, CancellationToken cancellationToken)
  {
    throw new NotImplementedException(); // TODO(fpion): implement
  }

  protected abstract IQueryBuilder From(TableId source);
}
