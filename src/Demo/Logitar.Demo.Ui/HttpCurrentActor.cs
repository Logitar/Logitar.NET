using Logitar.Identity.Core.Models;
using Logitar.Identity.EntityFrameworkCore.SqlServer;

namespace Logitar.Demo.Ui;

internal class HttpCurrentActor : ICurrentActor
{
  public Actor Actor { get; } = new();
}
