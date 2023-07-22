using Logitar.Identity.Core.Models;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer;

public interface ICurrentActor
{
  Actor Actor { get; }
}
