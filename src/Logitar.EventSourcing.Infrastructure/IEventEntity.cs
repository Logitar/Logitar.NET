namespace Logitar.EventSourcing.Infrastructure;

public interface IEventEntity
{
  Guid Id { get; }

  string EventType { get; }
  string EventData { get; }
}
