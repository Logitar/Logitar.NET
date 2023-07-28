using Logitar.Identity.Domain.Roles.Events;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Actors;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;

public record RoleEntity : AggregateEntity
{
  public RoleEntity(RoleCreatedEvent created, ActorEntity actor) : base(created, actor)
  {
    TenantId = created.TenantId;

    UniqueName = created.UniqueName;
  }

  private RoleEntity() : base()
  {
  }

  public int RoleId { get; private set; }

  public string? TenantId { get; private set; }

  public string UniqueName { get; private set; } = string.Empty;
  public string UniqueNameNormalized
  {
    get => UniqueName.ToUpper();
    private set { }
  }
  public string? DisplayName { get; private set; }
  public string? Description { get; private set; }

  public List<ApiKeyEntity> ApiKeys { get; private set; } = new();

  public void Update(RoleUpdatedEvent updated, ActorEntity actor)
  {
    base.Update(updated, actor);

    if (updated.UniqueName != null)
    {
      UniqueName = updated.UniqueName;
    }
    if (updated.DisplayName != null)
    {
      DisplayName = updated.DisplayName.Value;
    }
    if (updated.Description != null)
    {
      Description = updated.Description.Value;
    }
  }
}
