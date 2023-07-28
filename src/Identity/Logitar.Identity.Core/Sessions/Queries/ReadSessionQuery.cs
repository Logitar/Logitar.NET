using Logitar.Identity.Core.Sessions.Models;
using MediatR;

namespace Logitar.Identity.Core.Sessions.Queries;

public record ReadSessionQuery : IRequest<Session?>
{
  public ReadSessionQuery(string? id)
  {
    Id = id;
  }

  public string? Id { get; }
}
