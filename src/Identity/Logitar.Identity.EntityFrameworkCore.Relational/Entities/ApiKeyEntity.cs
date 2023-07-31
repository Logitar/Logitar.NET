using Logitar.Identity.Domain;
using Logitar.Identity.Domain.ApiKeys.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Actors;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Entities;

public record ApiKeyEntity : AggregateEntity, ICustomAttributesProvider
{
  public ApiKeyEntity(ApiKeyCreatedEvent created, ActorEntity actor) : base(created, actor)
  {
    Secret = created.Secret.Encode();

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

  public DateTime? AuthenticatedOn { get; private set; }

  public string? CustomAttributes { get; private set; }

  public List<RoleEntity> Roles { get; private set; } = new();

  public void Authenticate(ApiKeyAuthenticatedEvent authenticated) => AuthenticatedOn = authenticated.OccurredOn.ToUniversalTime();

  public void Update(ApiKeyUpdatedEvent updated, ActorEntity actor, IEnumerable<RoleEntity> roles)
  {
    Update(updated, actor);

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

    CustomAttributes = this.UpdateCustomAttributes(updated.CustomAttributes);

    Dictionary<string, RoleEntity> rolesById = roles.ToDictionary(x => x.AggregateId, x => x);
    foreach (var (roleId, action) in updated.Roles)
    {
      RoleEntity role = rolesById[roleId];

      switch (action)
      {
        case CollectionAction.Add:
          Roles.Add(role);
          break;
        case CollectionAction.Remove:
          Roles.Remove(role);
          break;
      }
    }
  }
}
