namespace Logitar.Identity.EntityFrameworkCore.Relational.Constants;

public static class Actor
{
  public static readonly string DefaultId = new Core.Models.Actor().Id;
  public const int SerializedLength = 3000;
}
