//using Logitar.Data;

//namespace Logitar.EventSourcing.SqlServer;

//public static class Events
//{
//  public static TableId Table => new(nameof(Events));

//  public static ColumnId ActorId => new(nameof(EventEntity.ActorId), Table);
//  public static ColumnId AggregateId => new(nameof(EventEntity.AggregateId), Table);
//  public static ColumnId AggregateType => new(nameof(EventEntity.AggregateType), Table);
//  public static ColumnId DeleteAction => new(nameof(EventEntity.DeleteAction), Table);
//  public static ColumnId EventData => new(nameof(EventEntity.EventData), Table);
//  public static ColumnId EventId => new(nameof(EventEntity.EventId), Table);
//  public static ColumnId EventType => new(nameof(EventEntity.EventType), Table);
//  public static ColumnId Id => new(nameof(EventEntity.Id), Table);
//  public static ColumnId OccurredOn => new(nameof(EventEntity.OccurredOn), Table);
//  public static ColumnId Version => new(nameof(EventEntity.Version), Table);

//  public static ColumnId[] All => new[] { EventId, Id, ActorId, OccurredOn, Version, DeleteAction,
//    AggregateType, AggregateId, EventType, EventData };
//}
