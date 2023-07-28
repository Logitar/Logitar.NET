using Logitar.Identity.Core.Sessions.Models;
using Logitar.Identity.Core.Sessions.Payloads;
using Logitar.Identity.Domain;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users;
using MediatR;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.Core.Sessions.Commands;

public class SignInCommandHandler : IRequestHandler<SignInCommand, Session>
{
  private readonly IMediator _mediator;
  private readonly IUserRepository _userRepository;
  private readonly IOptions<UserSettings> _userSettings;

  public SignInCommandHandler(IMediator mediator, IUserRepository userRepository,
    IOptions<UserSettings> userSetings)
  {
    _mediator = mediator;
    _userRepository = userRepository;
    _userSettings = userSetings;
  }

  public async Task<Session> Handle(SignInCommand command, CancellationToken cancellationToken)
  {
    SignInPayload payload = command.Payload;
    UserSettings userSettings = _userSettings.Value;

    UserAggregate? user = await _userRepository.LoadAsync(payload.TenantId, payload.UniqueName, cancellationToken);
    if (user == null && userSettings.RequireUniqueEmail)
    {
      EmailAddress email = new(payload.UniqueName);
      IEnumerable<UserAggregate> users = await _userRepository.LoadAsync(payload.TenantId, email, cancellationToken);
      if (users.Count() == 1)
      {
        user = users.Single();
      }
    }

    if (user == null)
    {
      StringBuilder message = new();
      message.AppendLine("The specified user could not be found.");
      message.Append("TenantId: ").AppendLine(payload.TenantId);
      message.Append("UniqueName: ").AppendLine(payload.UniqueName);
      throw new InvalidCredentialsException(message.ToString());
    }

    return await _mediator.Send(new SignInUserCommand(user, payload.Password, payload.IsPersistent), cancellationToken);
  }
}
