namespace Logitar.Identity.EntityFrameworkCore.Relational.Entities;

public record UserRoleEntity
{
  public int UserId { get; set; }
  public int RoleId { get; set; }
}
