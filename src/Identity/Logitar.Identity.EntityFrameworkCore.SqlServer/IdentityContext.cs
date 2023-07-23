using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer;

public class IdentityContext : DbContext
{
  public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
  {
  }

  public DbSet<BlacklistedTokenEntity> TokenBlacklist { get; private set; } = null!;
  public DbSet<UserEntity> Users { get; private set; } = null!;

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityContext).Assembly);
  }
}
