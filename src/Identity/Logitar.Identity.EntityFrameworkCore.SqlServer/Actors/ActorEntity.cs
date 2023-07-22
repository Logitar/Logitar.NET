using Logitar.Identity.Core.Models;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Actors;

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

  public string Serialize() => JsonSerializer.Serialize(this);
}
