using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Core.Users.Payloads;
using MediatR;

namespace Logitar.Identity.Core.Users.Queries;

public record SearchUserQuery : IRequest<SearchResults<User>>
{
  public SearchUserQuery(SearchUserPayload payload)
  {
    Payload = payload;
  }

  public SearchUserPayload Payload { get; }
}
