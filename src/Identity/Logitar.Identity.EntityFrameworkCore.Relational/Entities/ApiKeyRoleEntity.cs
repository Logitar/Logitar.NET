namespace Logitar.Identity.EntityFrameworkCore.Relational.Entities;

public record ApiKeyRoleEntity
{
  public int ApiKeyId { get; private set; }
  public int RoleId { get; private set; }
}
