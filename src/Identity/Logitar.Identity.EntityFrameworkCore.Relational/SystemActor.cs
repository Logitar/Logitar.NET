using Logitar.Identity.Core.Models;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

public class SystemActor : ICurrentActor
{
  public Actor Actor { get; } = new();
}
