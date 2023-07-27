using AutoMapper;
using Logitar.Data;
using Logitar.Data.SqlServer;
using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Roles;
using Logitar.Identity.Core.Roles.Models;
using Logitar.Identity.Core.Roles.Payloads;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Constants;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Queriers;

public class RoleQuerier : IRoleQuerier
{
  private readonly IMapper _mapper;
  private readonly DbSet<RoleEntity> _roles;

  public RoleQuerier(IdentityContext context, IMapper mapper)
  {
    _mapper = mapper;
    _roles = context.Roles;
  }

  public async Task<Role> ReadAsync(RoleAggregate role, CancellationToken cancellationToken)
  {
    return await ReadAsync(role.Id.Value, cancellationToken)
      ?? throw new EntityNotFoundException<RoleEntity>(role.Id.Value);
  }
  public async Task<Role?> ReadAsync(string id, CancellationToken cancellationToken)
  {
    RoleEntity? role = await _roles.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == id, cancellationToken);

    return _mapper.Map<Role?>(role);
  }

  public async Task<Role?> ReadAsync(string? tenantId, string uniqueName, CancellationToken cancellationToken)
  {
    tenantId = tenantId?.CleanTrim();
    string uniqueNameNormalized = uniqueName.Trim().ToUpper();

    RoleEntity? role = await _roles.AsNoTracking()
      .SingleOrDefaultAsync(x => x.TenantId == tenantId && x.UniqueNameNormalized == uniqueNameNormalized, cancellationToken);

    return _mapper.Map<Role?>(role);
  }

  public async Task<SearchResults<Role>> SearchAsync(SearchRolePayload payload, CancellationToken cancellationToken)
  {
    IQueryBuilder builder = SqlServerQueryBuilder.From(Db.Roles.Table)
      .SelectAll(Db.Roles.Table)
      .ApplyTextSearch(payload.Id, Db.Roles.AggregateId)
      .ApplyTextSearch(payload.Search, Db.Roles.UniqueName, Db.Roles.DisplayName)
      .ApplyTextSearch(payload.TenantId, Db.Roles.TenantId);

    IQueryable<RoleEntity> query = _roles.FromQuery(builder.Build())
      .AsNoTracking();

    long total = await query.LongCountAsync(cancellationToken);

    IOrderedQueryable<RoleEntity>? ordered = null;
    foreach (RoleSortOption sort in payload.Sort)
    {
      switch (sort.Field)
      {
        case RoleSort.DisplayName:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.DisplayName) : ordered.ThenBy(x => x.DisplayName));
          break;
        case RoleSort.UniqueName:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UniqueName) : query.OrderBy(x => x.UniqueName))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UniqueName) : ordered.ThenBy(x => x.UniqueName));
          break;
        case RoleSort.UpdatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UpdatedOn) : query.OrderBy(x => x.UpdatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UpdatedOn) : ordered.ThenBy(x => x.UpdatedOn));
          break;
      }
    }
    query = ordered ?? query;

    query = query.ApplyPaging(payload);

    RoleEntity[] roles = await query.ToArrayAsync(cancellationToken);
    IEnumerable<Role> items = _mapper.Map<IEnumerable<Role>>(roles);

    return new SearchResults<Role>(items, total);
  }
}
