using Logitar.Identity.Core.Models;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Actors;

public record ActorEntity
{
  public string Type { get; set; } = "System";
  public bool IsDeleted { get; set; }

  public string DisplayName { get; set; } = "System";
  public string? EmailAddress { get; set; }
  public string? PictureUrl { get; set; }

  public static Actor? Deserialize(string? id, string? json)
  {
    if (id == null || json == null)
    {
      return null;
    }

    ActorEntity? actor = JsonSerializer.Deserialize<ActorEntity>(json);

    return actor == null ? null : new Actor
    {
      Id = id,
      Type = actor.Type,
      IsDeleted = actor.IsDeleted,
      DisplayName = actor.DisplayName,
      EmailAddress = actor.EmailAddress,
      PictureUrl = actor.PictureUrl
    };
  }

  public static ActorEntity From(Actor actor) => new()
  {
    Type = actor.Type,
    IsDeleted = actor.IsDeleted,
    DisplayName = actor.DisplayName,
    EmailAddress = actor.EmailAddress,
    PictureUrl = actor.PictureUrl
  };
  public static ActorEntity From(ApiKeyEntity apiKey, bool isDeleted = false) => new()
  {
    Type = "ApiKey",
    IsDeleted = isDeleted,
    DisplayName = apiKey.Title
  };
  public static ActorEntity From(UserEntity user, bool isDeleted = false) => new()
  {
    Type = "User",
    IsDeleted = isDeleted,
    DisplayName = user.FullName ?? user.UniqueName,
    EmailAddress = user.EmailAddress,
    PictureUrl = user.Picture
  };

  public string Serialize() => JsonSerializer.Serialize(this);
}
