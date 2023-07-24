using AutoMapper;
using Logitar.Identity.Core.Sessions;
using Logitar.Identity.Core.Sessions.Models;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Queries;

public class SessionQuerier : ISessionQuerier
{
  private readonly IMapper _mapper;
  private readonly DbSet<SessionEntity> _sessions;

  public SessionQuerier(IdentityContext context, IMapper mapper)
  {
    _mapper = mapper;
    _sessions = context.Sessions;
  }

  public async Task<Session> ReadAsync(SessionAggregate session, CancellationToken cancellationToken)
  {
    return await ReadAsync(session.Id.Value, cancellationToken)
      ?? throw new EntityNotFoundException<SessionEntity>(session.Id.Value);
  }
  public async Task<Session?> ReadAsync(string id, CancellationToken cancellationToken)
  {
    SessionEntity? session = await _sessions.AsNoTracking()
      .Include(x => x.User)
      .SingleOrDefaultAsync(x => x.AggregateId == id, cancellationToken);

    return _mapper.Map<Session?>(session);
  }
}
