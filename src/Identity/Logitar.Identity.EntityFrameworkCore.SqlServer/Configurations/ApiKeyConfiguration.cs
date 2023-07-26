﻿using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Configurations;

public class ApiKeyConfiguration : AggregateConfiguration<ApiKeyEntity>, IEntityTypeConfiguration<ApiKeyEntity>
{
  public override void Configure(EntityTypeBuilder<ApiKeyEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(nameof(IdentityContext.ApiKeys));
    builder.HasKey(x => x.ApiKeyId);

    builder.HasIndex(x => x.TenantId);
    builder.HasIndex(x => x.Title);
    builder.HasIndex(x => x.ExpiresOn);

    builder.Property(x => x.TenantId).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.Secret).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.Title).HasMaxLength(byte.MaxValue);
  }
}
