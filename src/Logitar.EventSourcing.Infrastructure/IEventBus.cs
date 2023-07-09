namespace Logitar.EventSourcing.Infrastructure;

public interface IEventBus
{
  Task PublishAsync(DomainEvent change, CancellationToken cancellationToken = default);
}
