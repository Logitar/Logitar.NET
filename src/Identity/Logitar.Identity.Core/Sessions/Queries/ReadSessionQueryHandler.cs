using Logitar.Identity.Core.Sessions.Models;
using MediatR;

namespace Logitar.Identity.Core.Sessions.Queries;

public class ReadSessionQueryHandler : IRequestHandler<ReadSessionQuery, Session?>
{
  private readonly ISessionQuerier _sessionQuerier;

  public ReadSessionQueryHandler(ISessionQuerier sessionQuerier)
  {
    _sessionQuerier = sessionQuerier;
  }

  public async Task<Session?> Handle(ReadSessionQuery query, CancellationToken cancellationToken)
  {
    Dictionary<string, Session> sessions = new(capacity: 1);

    if (query.Id != null)
    {
      Session? session = await _sessionQuerier.ReadAsync(query.Id, cancellationToken);
      if (session != null)
      {
        sessions[session.Id] = session;
      }
    }

    if (sessions.Count > 1)
    {
      throw new TooManyResultsException<Session>(expected: 1, actual: sessions.Count);
    }

    return sessions.Values.SingleOrDefault();
  }
}
