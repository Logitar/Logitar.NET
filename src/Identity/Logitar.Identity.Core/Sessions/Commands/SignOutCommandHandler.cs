using Logitar.EventSourcing;
using Logitar.Identity.Core.Sessions.Models;
using Logitar.Identity.Domain.Sessions;
using MediatR;

namespace Logitar.Identity.Core.Sessions.Commands;

public class SignOutCommandHandler : IRequestHandler<SignOutCommand, Session?>
{
  private readonly ISessionQuerier _sessionQuerier;
  private readonly ISessionRepository _sessionRepository;

  public SignOutCommandHandler(ISessionQuerier sessionQuerier, ISessionRepository sessionRepository)
  {
    _sessionQuerier = sessionQuerier;
    _sessionRepository = sessionRepository;
  }

  public async Task<Session?> Handle(SignOutCommand command, CancellationToken cancellationToken)
  {
    AggregateId sessionId = command.Id.GetAggregateId(nameof(command.Id));
    SessionAggregate? session = await _sessionRepository.LoadAsync(sessionId, cancellationToken);
    if (session == null)
    {
      return null;
    }

    session.SignOut();

    await _sessionRepository.SaveAsync(session, cancellationToken);

    return await _sessionQuerier.ReadAsync(session, cancellationToken);
  }
}
