using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Sessions.Commands;
using Logitar.Identity.Core.Sessions.Models;
using Logitar.Identity.Core.Sessions.Payloads;
using Logitar.Identity.Core.Sessions.Queries;
using MediatR;

namespace Logitar.Identity.Core.Sessions;

public class SessionFacade : ISessionFacade
{
  private readonly IMediator _mediator;

  public SessionFacade(IMediator mediator)
  {
    _mediator = mediator;
  }

  public virtual async Task<Session> CreateAsync(CreateSessionPayload payload, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new CreateSessionCommand(payload), cancellationToken);
  }

  public virtual async Task<Session?> ReadAsync(string? id, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new ReadSessionQuery(id), cancellationToken);
  }

  public virtual async Task<Session> RenewAsync(RenewSessionPayload payload, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new RenewSessionCommand(payload), cancellationToken);
  }

  public virtual async Task<SearchResults<Session>> SearchAsync(SearchSessionPayload payload, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new SearchSessionQuery(payload), cancellationToken);
  }

  public virtual async Task<Session> SignInAsync(SignInPayload payload, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new SignInCommand(payload), cancellationToken);
  }

  public virtual async Task<Session?> SignOutAsync(string id, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new SignOutCommand(id), cancellationToken);
  }
}
