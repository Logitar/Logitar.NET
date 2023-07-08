namespace Logitar.EventSourcing.EntityFrameworkCore.Relational;

/// <summary>
/// TODO(fpion): document
/// </summary>
public interface IEventBus
{
  Task PublishAsync(DomainEvent change, CancellationToken cancellationToken = default);
}
