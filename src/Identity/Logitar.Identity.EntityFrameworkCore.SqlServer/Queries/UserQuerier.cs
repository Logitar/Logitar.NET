using AutoMapper;
using Logitar.Identity.Core.Users;
using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Domain.Users;
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
}
