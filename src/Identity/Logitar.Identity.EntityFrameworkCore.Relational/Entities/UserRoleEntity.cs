namespace Logitar.Identity.EntityFrameworkCore.Relational.Entities;

public record UserRoleEntity
{
  public int UserId { get; private set; }
  public int RoleId { get; private set; }
}
