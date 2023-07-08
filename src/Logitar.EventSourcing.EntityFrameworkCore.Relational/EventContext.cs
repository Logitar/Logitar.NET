using Microsoft.EntityFrameworkCore;

namespace Logitar.EventSourcing.EntityFrameworkCore.Relational;

/// <summary>
/// TODO(fpion): document
/// </summary>
public class EventContext : DbContext
{
  public EventContext(DbContextOptions<EventContext> options) : base(options)
  {
  }

  public DbSet<EventEntity> Events { get; private set; } = null!;

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventContext).Assembly);
  }
}
