using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Sessions.Models;
using Logitar.Identity.Core.Sessions.Payloads;
using MediatR;

namespace Logitar.Identity.Core.Sessions.Queries;

public record SearchSessionQuery : IRequest<SearchResults<Session>>
{
  public SearchSessionQuery(SearchSessionPayload payload)
  {
    Payload = payload;
  }

  public SearchSessionPayload Payload { get; }
}
