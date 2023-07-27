using AutoMapper;
using Logitar.Data;
using Logitar.Data.SqlServer;
using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Sessions;
using Logitar.Identity.Core.Sessions.Models;
using Logitar.Identity.Core.Sessions.Payloads;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Constants;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Queriers;

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

  public async Task<SearchResults<Session>> SearchAsync(SearchSessionPayload payload, CancellationToken cancellationToken)
  {
    IQueryBuilder builder = SqlServerQueryBuilder.From(Db.Sessions.Table)
      .SelectAll(Db.Sessions.Table)
      .Join(Db.Users.UserId, Db.Sessions.UserId)
      .ApplyTextSearch(payload.Id, Db.Sessions.AggregateId)
      .ApplyTextSearch(payload.TenantId, Db.Users.TenantId)
      .ApplyTextSearch(payload.UserId, Db.Users.AggregateId);

    if (payload.IsPersistent.HasValue)
    {
      builder = builder.Where(Db.Sessions.IsPersistent, Operators.IsEqualTo(payload.IsPersistent.Value));
    }
    if (payload.IsActive.HasValue)
    {
      builder = builder.Where(Db.Sessions.IsActive, Operators.IsEqualTo(payload.IsActive.Value));
    }

    IQueryable<SessionEntity> query = _sessions.FromQuery(builder.Build())
      .Include(x => x.User)
      .AsNoTracking();

    long total = await query.LongCountAsync(cancellationToken);

    IOrderedQueryable<SessionEntity>? ordered = null;
    foreach (SessionSortOption sort in payload.Sort)
    {
      switch (sort.Field)
      {
        case SessionSort.SignedOutOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.SignedOutOn) : query.OrderBy(x => x.SignedOutOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.SignedOutOn) : ordered.ThenBy(x => x.SignedOutOn));
          break;
        case SessionSort.UpdatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UpdatedOn) : query.OrderBy(x => x.UpdatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UpdatedOn) : ordered.ThenBy(x => x.UpdatedOn));
          break;
      }
    }
    query = ordered ?? query;

    query = query.ApplyPaging(payload);

    SessionEntity[] sessions = await query.ToArrayAsync(cancellationToken);
    IEnumerable<Session> items = _mapper.Map<IEnumerable<Session>>(sessions);

    return new SearchResults<Session>(items, total);
  }
}
