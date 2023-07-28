namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;

public record ApiKeyRoleEntity
{
  public int ApiKeyId { get; private set; }
  public int RoleId { get; private set; }
}
