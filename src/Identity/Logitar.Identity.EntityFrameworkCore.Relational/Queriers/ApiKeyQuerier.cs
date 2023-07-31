using AutoMapper;
using Logitar.Data;
using Logitar.Identity.Core.ApiKeys;
using Logitar.Identity.Core.ApiKeys.Models;
using Logitar.Identity.Core.ApiKeys.Payloads;
using Logitar.Identity.Core.Models;
using Logitar.Identity.Domain.ApiKeys;
using Logitar.Identity.EntityFrameworkCore.Relational.Constants;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Queriers;

public class ApiKeyQuerier : IApiKeyQuerier
{
  private readonly DbSet<ApiKeyEntity> _apiKeys;
  private readonly IMapper _mapper;
  private readonly IQueryHelper _queryHelper;

  public ApiKeyQuerier(IdentityContext context, IMapper mapper, IQueryHelper queryHelper)
  {
    _apiKeys = context.ApiKeys;
    _mapper = mapper;
    _queryHelper = queryHelper;
  }

  public async Task<ApiKey> ReadAsync(ApiKeyAggregate apiKey, CancellationToken cancellationToken)
  {
    return await ReadAsync(apiKey.Id.Value, cancellationToken)
      ?? throw new EntityNotFoundException<ApiKeyEntity>(apiKey.Id.Value);
  }
  public async Task<ApiKey?> ReadAsync(string id, CancellationToken cancellationToken)
  {
    ApiKeyEntity? apiKey = await _apiKeys.AsNoTracking()
      .Include(x => x.Roles)
      .SingleOrDefaultAsync(x => x.AggregateId == id, cancellationToken);

    return _mapper.Map<ApiKey?>(apiKey);
  }

  public async Task<SearchResults<ApiKey>> SearchAsync(SearchApiKeyPayload payload, CancellationToken cancellationToken)
  {
    IQueryBuilder builder = _queryHelper.From(Db.ApiKeys.Table).SelectAll(Db.ApiKeys.Table);
    builder = _queryHelper.ApplyTextSearch(builder, payload.Id, Db.ApiKeys.AggregateId);
    builder = _queryHelper.ApplyTextSearch(builder, payload.Search, Db.ApiKeys.Title);
    builder = _queryHelper.ApplyTextSearch(builder, payload.TenantId, Db.ApiKeys.TenantId);

    DateTime now = DateTime.UtcNow;
    switch (payload.IsExpired)
    {
      case true:
        builder = builder.Where(new OperatorCondition(Db.ApiKeys.ExpiresOn, Operators.IsLessThanOrEqualTo(now)));
        break;
      case false:
        builder = builder.WhereOr(new OperatorCondition(Db.ApiKeys.ExpiresOn, Operators.IsNull()),
          new OperatorCondition(Db.ApiKeys.ExpiresOn, Operators.IsGreaterThan(now)));
        break;
    }

    IQueryable<ApiKeyEntity> query = _apiKeys.FromQuery(builder.Build())
      .Include(x => x.Roles)
      .AsNoTracking();

    long total = await query.LongCountAsync(cancellationToken);

    IOrderedQueryable<ApiKeyEntity>? ordered = null;
    foreach (ApiKeySortOption sort in payload.Sort)
    {
      switch (sort.Field)
      {
        case ApiKeySort.ExpiresOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.ExpiresOn) : query.OrderBy(x => x.ExpiresOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.ExpiresOn) : ordered.ThenBy(x => x.ExpiresOn));
          break;
        case ApiKeySort.Title:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.Title) : query.OrderBy(x => x.Title))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.Title) : ordered.ThenBy(x => x.Title));
          break;
        case ApiKeySort.UpdatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UpdatedOn) : query.OrderBy(x => x.UpdatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UpdatedOn) : ordered.ThenBy(x => x.UpdatedOn));
          break;
      }
    }
    query = ordered ?? query;

    query = query.ApplyPaging(payload);

    ApiKeyEntity[] users = await query.ToArrayAsync(cancellationToken);
    IEnumerable<ApiKey> items = _mapper.Map<IEnumerable<ApiKey>>(users);

    return new SearchResults<ApiKey>(items, total);
  }
}
