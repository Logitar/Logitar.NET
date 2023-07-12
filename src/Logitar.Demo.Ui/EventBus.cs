using Logitar.EventSourcing;
using Logitar.EventSourcing.Infrastructure;

namespace Logitar.Demo.Ui;

internal class EventBus : IEventBus
{
  public Task PublishAsync(DomainEvent change, CancellationToken cancellationToken)
  {
    return Task.CompletedTask;
  }
}
