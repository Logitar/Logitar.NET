using Logitar.Identity.Domain.ApiKeys.Events;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Actors;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Handlers.ApiKeys;

public class ApiKeyDeletedEventHandler : INotificationHandler<ApiKeyDeletedEvent>
{
  private readonly IActorService _actorService;
  private readonly IdentityContext _context;

  public ApiKeyDeletedEventHandler(IActorService actorService, IdentityContext context)
  {
    _actorService = actorService;
    _context = context;
  }

  public async Task Handle(ApiKeyDeletedEvent notification, CancellationToken cancellationToken)
  {
    ApiKeyEntity? apiKey = await _context.ApiKeys
      .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken);
    if (apiKey == null)
    {
      return;
    }

    _context.ApiKeys.Remove(apiKey);
    await _context.SaveChangesAsync(cancellationToken);

    await _actorService.DeleteAsync(apiKey, cancellationToken);
  }
}
