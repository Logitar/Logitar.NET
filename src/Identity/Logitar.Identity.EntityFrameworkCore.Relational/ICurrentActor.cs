using Logitar.Identity.Core.Models;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

public interface ICurrentActor
{
  Actor Actor { get; }
}
