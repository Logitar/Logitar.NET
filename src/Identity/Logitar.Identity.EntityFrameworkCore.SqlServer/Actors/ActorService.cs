using Logitar.EventSourcing;
using Logitar.Identity.Core.Models;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Actors;

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

    // TODO(fpion): API key actors

    throw new ActorNotFoundException(change.ActorId);
  }

  public async Task DeleteAsync(UserEntity user, CancellationToken cancellationToken)
    => await UpdateAsync(user.AggregateId, ActorEntity.From(user, isDeleted: true), cancellationToken);
  public async Task UpdateAsync(UserEntity user, CancellationToken cancellationToken)
    => await UpdateAsync(user.AggregateId, ActorEntity.From(user), cancellationToken);
  private async Task UpdateAsync(string id, ActorEntity actor, CancellationToken cancellationToken)
  {
    string serialized = actor.Serialize();

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
