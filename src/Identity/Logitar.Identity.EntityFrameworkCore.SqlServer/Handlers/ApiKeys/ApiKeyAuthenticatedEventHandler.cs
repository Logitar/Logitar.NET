using Logitar.Identity.Domain.ApiKeys.Events;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Handlers.ApiKeys;

public class ApiKeyAuthenticatedEventHandler : INotificationHandler<ApiKeyAuthenticatedEvent>
{
  private readonly IdentityContext _context;

  public ApiKeyAuthenticatedEventHandler(IdentityContext context)
  {
    _context = context;
  }

  public async Task Handle(ApiKeyAuthenticatedEvent notification, CancellationToken cancellationToken)
  {
    ApiKeyEntity apikey = await _context.ApiKeys
      .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken)
      ?? throw new EntityNotFoundException<ApiKeyEntity>(notification.AggregateId.Value);

    apikey.Authenticate(notification);

    await _context.SaveChangesAsync(cancellationToken);
  }
}
