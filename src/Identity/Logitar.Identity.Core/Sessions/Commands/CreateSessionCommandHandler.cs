using Logitar.EventSourcing;
using Logitar.Identity.Core.Sessions.Models;
using Logitar.Identity.Core.Sessions.Payloads;
using Logitar.Identity.Domain.Users;
using MediatR;

namespace Logitar.Identity.Core.Sessions.Commands;

public class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand, Session>
{
  private readonly IMediator _mediator;
  private readonly IUserRepository _userRepository;

  public CreateSessionCommandHandler(IMediator mediator, IUserRepository userRepository)
  {
    _mediator = mediator;
    _userRepository = userRepository;
  }

  public async Task<Session> Handle(CreateSessionCommand command, CancellationToken cancellationToken)
  {
    CreateSessionPayload payload = command.Payload;

    AggregateId userId = payload.UserId.GetAggregateId(nameof(payload.UserId));
    UserAggregate user = await _userRepository.LoadAsync(userId, cancellationToken)
      ?? throw new AggregateNotFoundException<UserAggregate>(payload.UserId, nameof(payload.UserId));

    return await _mediator.Send(new SignInUserCommand(user, password: null, payload.IsPersistent,
      payload.CustomAttributes), cancellationToken);
  }
}
