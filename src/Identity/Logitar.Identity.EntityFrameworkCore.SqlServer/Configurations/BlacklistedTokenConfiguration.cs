﻿using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Configurations;

public class BlacklistedTokenConfiguration : IEntityTypeConfiguration<BlacklistedTokenEntity>
{
  public void Configure(EntityTypeBuilder<BlacklistedTokenEntity> builder)
  {
    builder.ToTable(nameof(IdentityContext.TokenBlacklist));
    builder.HasKey(x => x.BlacklistedTokenId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.ExpiresOn);
  }
}
