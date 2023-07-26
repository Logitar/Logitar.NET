using Logitar.Identity.Domain.ApiKeys.Events;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Actors;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;

public record ApiKeyEntity : AggregateEntity
{
  public ApiKeyEntity(ApiKeyCreatedEvent created, ActorEntity actor) : base(created, actor)
  {
    Secret = created.Secret.ToString();

    TenantId = created.TenantId;

    Title = created.Title;
  }

  private ApiKeyEntity() : base()
  {
  }

  public int ApiKeyId { get; private set; }

  public string Secret { get; private set; } = string.Empty;

  public string? TenantId { get; private set; }

  public string Title { get; private set; } = string.Empty;
  public string? Description { get; private set; }
  public DateTime? ExpiresOn { get; private set; }

  public void Update(ApiKeyUpdatedEvent updated, ActorEntity actor)
  {
    base.Update(updated, actor);

    if (updated.Title != null)
    {
      Title = updated.Title;
    }
    if (updated.Description != null)
    {
      Description = updated.Description.Value;
    }
    if (updated.ExpiresOn.HasValue)
    {
      ExpiresOn = updated.ExpiresOn.Value.ToUniversalTime();
    }
  }
}
