using AutoMapper;
using Logitar.Data;
using Logitar.Data.SqlServer;
using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Users;
using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Core.Users.Payloads;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Constants;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Queries;

public class UserQuerier : IUserQuerier
{
  private readonly IMapper _mapper;
  private readonly DbSet<UserEntity> _users;

  public UserQuerier(IdentityContext context, IMapper mapper)
  {
    _mapper = mapper;
    _users = context.Users;
  }

  public async Task<User> ReadAsync(UserAggregate user, CancellationToken cancellationToken)
  {
    return await ReadAsync(user.Id.Value, cancellationToken)
      ?? throw new EntityNotFoundException<UserEntity>(user.Id.Value);
  }
  public async Task<User?> ReadAsync(string id, CancellationToken cancellationToken)
  {
    UserEntity? user = await _users.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == id, cancellationToken);

    return _mapper.Map<User?>(user);
  }

  public async Task<User?> ReadAsync(string? tenantId, string uniqueName, CancellationToken cancellationToken)
  {
    tenantId = tenantId?.CleanTrim();
    string uniqueNameNormalized = uniqueName.Trim().ToUpper();

    UserEntity? user = await _users.AsNoTracking()
      .SingleOrDefaultAsync(x => x.TenantId == tenantId && x.UniqueNameNormalized == uniqueNameNormalized, cancellationToken);

    return _mapper.Map<User?>(user);
  }

  public async Task<IEnumerable<User>> ReadAsync(string? tenantId, IEmailAddress email, CancellationToken cancellationToken)
  {
    tenantId = tenantId?.CleanTrim();
    string emailAddressNormalized = email.Address.ToUpper();

    UserEntity[] users = await _users.AsNoTracking()
      .Where(x => x.TenantId == tenantId && x.EmailAddressNormalized == emailAddressNormalized)
      .ToArrayAsync(cancellationToken);

    return _mapper.Map<IEnumerable<User>>(users);
  }

  public async Task<SearchResults<User>> SearchAsync(SearchUserPayload payload, CancellationToken cancellationToken)
  {
    IQueryBuilder builder = SqlServerQueryBuilder.From(Db.Users.Table)
      .SelectAll(Db.Users.Table)
      .ApplyTextSearch(payload.Id, Db.Users.AggregateId)
      .ApplyTextSearch(payload.Search, Db.Users.UniqueName, Db.Users.AddressFormatted,
        Db.Users.EmailAddress, Db.Users.PhoneE164Formatted, Db.Users.FullName)
      .ApplyTextSearch(payload.TenantId, Db.Users.TenantId);

    if (payload.HasPassword.HasValue)
    {
      builder = builder.Where(Db.Users.HasPassword, Operators.IsEqualTo(payload.HasPassword.Value));
    }
    if (payload.IsConfirmed.HasValue)
    {
      builder = builder.Where(Db.Users.IsConfirmed, Operators.IsEqualTo(payload.IsConfirmed.Value));
    }
    if (payload.IsDisabled.HasValue)
    {
      builder = builder.Where(Db.Users.IsDisabled, Operators.IsEqualTo(payload.IsDisabled.Value));
    }

    IQueryable<UserEntity> query = _users.FromQuery(builder.Build())
      .AsNoTracking();

    long total = await query.LongCountAsync(cancellationToken);

    IOrderedQueryable<UserEntity>? ordered = null;
    foreach (UserSortOption sort in payload.Sort)
    {
      switch (sort.Field)
      {
        case UserSort.AuthenticatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.AuthenticatedOn) : query.OrderBy(x => x.AuthenticatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.AuthenticatedOn) : ordered.ThenBy(x => x.AuthenticatedOn));
          break;
        case UserSort.Birthdate:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.Birthdate) : query.OrderBy(x => x.Birthdate))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.Birthdate) : ordered.ThenBy(x => x.Birthdate));
          break;
        case UserSort.DisabledOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.DisabledOn) : query.OrderBy(x => x.DisabledOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.DisabledOn) : ordered.ThenBy(x => x.DisabledOn));
          break;
        case UserSort.FullName:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.FullName) : query.OrderBy(x => x.FullName))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.FullName) : ordered.ThenBy(x => x.FullName));
          break;
        case UserSort.LastFirstMiddleName:
          ordered = (ordered == null)
            ? (sort.IsDescending
              ? query.OrderByDescending(x => x.LastName).ThenByDescending(x => x.FirstName).ThenByDescending(x => x.MiddleName)
              : query.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ThenBy(x => x.MiddleName)
            ) : (sort.IsDescending
              ? ordered.ThenByDescending(x => x.LastName).ThenByDescending(x => x.FirstName).ThenByDescending(x => x.MiddleName)
              : ordered.ThenBy(x => x.LastName).ThenBy(x => x.FirstName).ThenBy(x => x.MiddleName)
            );
          break;
        case UserSort.PasswordChangedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.PasswordChangedOn) : query.OrderBy(x => x.PasswordChangedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.PasswordChangedOn) : ordered.ThenBy(x => x.PasswordChangedOn));
          break;
        case UserSort.UniqueName:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UniqueName) : query.OrderBy(x => x.UniqueName))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UniqueName) : ordered.ThenBy(x => x.UniqueName));
          break;
        case UserSort.UpdatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UpdatedOn) : query.OrderBy(x => x.UpdatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UpdatedOn) : ordered.ThenBy(x => x.UpdatedOn));
          break;
      }
    }
    query = ordered ?? query;

    query = query.ApplyPaging(payload);

    UserEntity[] users = await query.ToArrayAsync(cancellationToken);
    IEnumerable<User> items = _mapper.Map<IEnumerable<User>>(users);

    return new SearchResults<User>(items, total);
  }
}
