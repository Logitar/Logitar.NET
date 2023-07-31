namespace Logitar.Identity.EntityFrameworkCore.Relational.Entities;

public class BlacklistedTokenEntity
{
  public BlacklistedTokenEntity(Guid id, DateTime? expiresOn = null)
  {
    Id = id;
    ExpiresOn = expiresOn?.ToUniversalTime();
  }

  private BlacklistedTokenEntity()
  {
  }

  public long BlacklistedTokenId { get; private set; }

  public Guid Id { get; private set; }

  public DateTime? ExpiresOn { get; private set; }
}
