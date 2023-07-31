using Logitar.EventSourcing;
using Logitar.Identity.Core.Models;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Actors;

public class ActorService : IActorService
{
  private static readonly Actor _system = new();

  private readonly IdentityContext _context;

  public ActorService(IdentityContext context)
  {
    _context = context;
  }

  public async Task<ActorEntity> FindAsync(DomainEvent change, CancellationToken cancellationToken)
  {
    if (change.ActorId == null || change.ActorId == _system.Id)
    {
      return ActorEntity.From(_system);
    }

    UserEntity? user = await _context.Users.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == change.ActorId, cancellationToken);
    if (user != null)
    {
      return ActorEntity.From(user);
    }

    ApiKeyEntity? apiKey = await _context.ApiKeys.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == change.ActorId, cancellationToken);
    if (apiKey != null)
    {
      return ActorEntity.From(apiKey);
    }

    throw new ActorNotFoundException(change.ActorId);
  }

  public async Task DeleteAsync(ApiKeyEntity apiKey, CancellationToken cancellationToken)
    => await UpdateAsync(apiKey.AggregateId, ActorEntity.From(apiKey, isDeleted: true), cancellationToken);
  public async Task UpdateAsync(ApiKeyEntity apiKey, CancellationToken cancellationToken)
    => await UpdateAsync(apiKey.AggregateId, ActorEntity.From(apiKey), cancellationToken);

  public async Task DeleteAsync(UserEntity user, CancellationToken cancellationToken)
    => await UpdateAsync(user.AggregateId, ActorEntity.From(user, isDeleted: true), cancellationToken);
  public async Task UpdateAsync(UserEntity user, CancellationToken cancellationToken)
    => await UpdateAsync(user.AggregateId, ActorEntity.From(user), cancellationToken);

  private async Task UpdateAsync(string id, ActorEntity actor, CancellationToken cancellationToken)
  {
    string serialized = actor.Serialize();

    ApiKeyEntity[] apiKeys = await _context.ApiKeys
      .Where(x => x.CreatedById == id || x.UpdatedById == id)
      .ToArrayAsync(cancellationToken);
    foreach (ApiKeyEntity apiKey in apiKeys)
    {
      apiKey.SetActor(id, serialized);
    }

    SessionEntity[] sessions = await _context.Sessions
      .Where(x => x.CreatedById == id || x.UpdatedById == id || x.SignedOutById == id)
      .ToArrayAsync(cancellationToken);
    foreach (SessionEntity session in sessions)
    {
      session.SetActor(id, serialized);
    }

    UserEntity[] users = await _context.Users
      .Where(x => x.CreatedById == id || x.UpdatedById == id || x.PasswordChangedById == id
        || x.DisabledById == id || x.AddressVerifiedById == id || x.EmailVerifiedById == id
        || x.PhoneVerifiedById == id)
      .ToArrayAsync(cancellationToken);
    foreach (UserEntity user in users)
    {
      user.SetActor(id, serialized);
    }
  }
}
