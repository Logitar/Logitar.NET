using Logitar.Identity.Core.Tokens;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer;

public class TokenBlacklist : ITokenBlacklist
{
  private readonly IdentityContext _context;

  public TokenBlacklist(IdentityContext context)
  {
    _context = context;
  }

  public async Task BlacklistAsync(IEnumerable<Guid> ids, DateTime? expiresOn, CancellationToken cancellationToken)
  {
    IEnumerable<BlacklistedTokenEntity> entities = ids.Select(id => new BlacklistedTokenEntity(id, expiresOn));

    _context.TokenBlacklist.AddRange(entities);
    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task<IEnumerable<Guid>> GetBlacklistedAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
  {
    return await _context.TokenBlacklist.AsNoTracking()
      .Where(x => ids.Contains(x.Id) && (x.ExpiresOn == null || x.ExpiresOn > DateTime.UtcNow))
      .Select(x => x.Id)
      .ToArrayAsync(cancellationToken);
  }
}
