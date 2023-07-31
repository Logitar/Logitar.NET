using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

public class IdentityContext : DbContext
{
  public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
  {
  }

  public DbSet<ApiKeyRoleEntity> ApiKeyRoles { get; private set; } = null!;
  public DbSet<ApiKeyEntity> ApiKeys { get; private set; } = null!;
  public DbSet<RoleEntity> Roles { get; private set; } = null!;
  public DbSet<SessionEntity> Sessions { get; private set; } = null!;
  public DbSet<BlacklistedTokenEntity> TokenBlacklist { get; private set; } = null!;
  public DbSet<UserRoleEntity> UserRoles { get; private set; } = null!;
  public DbSet<UserEntity> Users { get; private set; } = null!;

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityContext).Assembly);
  }
}
