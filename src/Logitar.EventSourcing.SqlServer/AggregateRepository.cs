//using Logitar.Data;
//using Logitar.Data.SqlServer;
//using Microsoft.Data.SqlClient;
//using System.Data;
//using System.Text;

//namespace Logitar.EventSourcing.SqlServer;

//public class AggregateRepository : IAggregateRepository, IDisposable
//{
//  private readonly SqlConnection _connection;

//  public AggregateRepository(IEventBus eventBus)
//  {
//    EventBus = eventBus;

//    _connection = new(connectionString: string.Empty); // TODO(fpion): implement
//  }

//  protected IEventBus EventBus { get; }

//  public void Dispose()
//  {
//    _connection.Dispose();
//    GC.SuppressFinalize(this);
//  }

//  public async Task<T?> LoadAsync<T>(AggregateId id, CancellationToken cancellationToken)
//    where T : AggregateRoot
//  {
//    return await LoadAsync<T>(id, includeDeleted: false, cancellationToken);
//  }
//  public async Task<T?> LoadAsync<T>(AggregateId id, long? version, CancellationToken cancellationToken)
//    where T : AggregateRoot
//  {
//    return await LoadAsync<T>(id, version, includeDeleted: false, cancellationToken);
//  }
//  public async Task<T?> LoadAsync<T>(AggregateId id, bool includeDeleted, CancellationToken cancellationToken)
//    where T : AggregateRoot
//  {
//    return await LoadAsync<T>(id, version: null, includeDeleted, cancellationToken);
//  }
//  public async Task<T?> LoadAsync<T>(AggregateId id, long? version, bool includeDeleted, CancellationToken cancellationToken)
//    where T : AggregateRoot
//  {
//    string aggregateId = id.Value;
//    string aggregateType = typeof(T).GetName();

//    IQuery query = SqlServerQueryBuilder.From(Events.Table)
//      .WhereAnd(new OperatorCondition(Events.AggregateType, Operators.IsEqualTo(aggregateType)),
//        new OperatorCondition(Events.AggregateId, Operators.IsEqualTo(aggregateId)))
//      .OrderBy(Events.Version)
//      .Select(Events.All)
//      .Build();

//    IEnumerable<EventEntity> events = await ReadEventsAsync(query, cancellationToken);

//    return Load<T>(events, includeDeleted).SingleOrDefault();
//  }

//  public async Task<IEnumerable<T>> LoadAsync<T>(CancellationToken cancellationToken)
//    where T : AggregateRoot
//  {
//    return await LoadAsync<T>(includeDeleted: false, cancellationToken);
//  }
//  public async Task<IEnumerable<T>> LoadAsync<T>(bool includeDeleted, CancellationToken cancellationToken)
//    where T : AggregateRoot
//  {
//    string aggregateType = typeof(T).GetName();

//    IQuery query = SqlServerQueryBuilder.From(Events.Table)
//      .Where(Events.AggregateType, Operators.IsEqualTo(aggregateType))
//      .OrderBy(Events.Version)
//      .Select(Events.All)
//      .Build();

//    IEnumerable<EventEntity> events = await ReadEventsAsync(query, cancellationToken);

//    return Load<T>(events, includeDeleted);
//  }

//  public async Task<IEnumerable<T>> LoadAsync<T>(IEnumerable<AggregateId> ids, CancellationToken cancellationToken)
//    where T : AggregateRoot
//  {
//    return await LoadAsync<T>(ids, includeDeleted: false, cancellationToken);
//  }
//  public async Task<IEnumerable<T>> LoadAsync<T>(IEnumerable<AggregateId> ids, bool includeDeleted, CancellationToken cancellationToken)
//    where T : AggregateRoot
//  {
//    HashSet<string> aggregateIds = ids.Select(id => id.Value).ToHashSet();
//    string aggregateType = typeof(T).GetName();

//    IQuery query = SqlServerQueryBuilder.From(Events.Table)
//      .WhereAnd(new OperatorCondition(Events.AggregateType, Operators.IsEqualTo(aggregateType)),
//        new OperatorCondition(Events.AggregateId, Operators.IsIn(aggregateIds.ToArray())))
//      .OrderBy(Events.Version)
//      .Select(Events.All)
//      .Build();

//    IEnumerable<EventEntity> events = await ReadEventsAsync(query, cancellationToken);

//    return Load<T>(events, includeDeleted);
//  }

//  public async Task SaveAsync(AggregateRoot aggregate, CancellationToken cancellationToken)
//  {
//    await SaveAsync(new[] { aggregate }, cancellationToken);
//  }
//  public async Task SaveAsync(IEnumerable<AggregateRoot> aggregates, CancellationToken cancellationToken)
//  {
//    EventEntity[] events = aggregates.SelectMany(EventEntity.FromChanges).ToArray();

//    if (_connection.State != ConnectionState.Open)
//    {
//      await _connection.OpenAsync(cancellationToken);
//    }

//    using SqlCommand command = _connection.CreateCommand();

//    StringBuilder text = new();
//    List<SqlParameter> parameters = new();

//    text.AppendLine("INSERT INTO [dbo].[Events] ([EventId], [Id], [ActorId], [OccurredOn], [Version], [DeleteAction], [AggregateType], [AggregateId], [EventType], [EventData]) VALUES");
//    for (int i = 0; i < events.Length; i++)
//    {
//      EventEntity entity = events[i];

//      string[] inserts = new[]
//      {
//        CreateParameter(parameters, entity.EventId),
//        CreateParameter(parameters, entity.Id),
//        entity.ActorId == null ? "NULL" : CreateParameter(parameters, entity.ActorId),
//        CreateParameter(parameters, entity.OccurredOn),
//        CreateParameter(parameters, entity.Version),
//        CreateParameter(parameters, entity.DeleteAction),
//        CreateParameter(parameters, entity.AggregateType),
//        CreateParameter(parameters, entity.AggregateId),
//        CreateParameter(parameters, entity.EventType),
//        CreateParameter(parameters, entity.EventData),
//      };
//      text.Append('(').Append(string.Join(", ", inserts)).Append(')');

//      text.Append(i < events.Length - 1 ? ',' : ';').AppendLine();
//    }

//    command.CommandText = text.ToString();
//    command.Parameters.AddRange(parameters.ToArray());
//    _ = await command.ExecuteNonQueryAsync(cancellationToken);

//    //foreach (AggregateRoot aggregate in aggregates)
//    //{
//    //  if (aggregate.HasChanges)
//    //  {
//    //    foreach (DomainEvent change in aggregate.Changes)
//    //    {
//    //      await EventBus.PublishAsync(change, cancellationToken);
//    //    }

//    //    aggregate.ClearChanges();
//    //  }
//    //}
//  }
//  private static string CreateParameter(ICollection<SqlParameter> parameters, object value)
//  {
//    string name = $"p{parameters.Count}";

//    SqlParameter parameter = new(name, value);
//    parameters.Add(parameter);

//    return name;
//  }

//  private async Task<IEnumerable<EventEntity>> ReadEventsAsync(IQuery query, CancellationToken cancellationToken)
//  {
//    List<EventEntity> events = new();

//    if (_connection.State != ConnectionState.Open)
//    {
//      await _connection.OpenAsync(cancellationToken);
//    }

//    using SqlCommand command = _connection.CreateCommand();
//    command.CommandText = query.Text;
//    command.Parameters.AddRange(query.Parameters.ToArray());

//    using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
//    if (reader.HasRows)
//    {
//      while (await reader.ReadAsync(cancellationToken))
//      {
//        events.Add(EventEntity.FromReader(reader));
//      }
//    }

//    return events;
//  }
//}
