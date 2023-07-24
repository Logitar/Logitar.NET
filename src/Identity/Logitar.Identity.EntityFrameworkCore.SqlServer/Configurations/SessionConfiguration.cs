using Logitar.Identity.EntityFrameworkCore.SqlServer.Constants;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Configurations;

public class SessionConfiguration : AggregateConfiguration<SessionEntity>, IEntityTypeConfiguration<SessionEntity>
{
  public override void Configure(EntityTypeBuilder<SessionEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(nameof(IdentityContext.Sessions));
    builder.HasKey(x => x.SessionId);

    builder.HasIndex(x => x.SignedOutById);

    builder.Property(x => x.Secret).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.SignedOutById).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.SignedOutBy).HasMaxLength(Actor.SerializedLength);

    builder.HasOne(x => x.User).WithMany(x => x.Sessions).OnDelete(DeleteBehavior.Restrict);
  }
}
