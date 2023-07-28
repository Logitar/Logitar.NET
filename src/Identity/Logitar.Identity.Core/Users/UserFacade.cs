using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Tokens.Models;
using Logitar.Identity.Core.Users.Commands;
using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Core.Users.Payloads;
using Logitar.Identity.Core.Users.Queries;
using MediatR;

namespace Logitar.Identity.Core.Users;

public class UserFacade : IUserFacade
{
  private readonly IMediator _mediator;

  public UserFacade(IMediator mediator)
  {
    _mediator = mediator;
  }

  public virtual async Task<User> AuthenticateAsync(AuthenticateUserPayload payload, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new AuthenticateUserCommand(payload), cancellationToken);
  }

  public virtual async Task<User?> ChangePasswordAsync(string id, ChangePasswordPayload payload, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new ChangePasswordCommand(id, payload), cancellationToken);
  }

  public virtual async Task<User> CreateAsync(CreateUserPayload payload, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new CreateUserCommand(payload), cancellationToken);
  }

  public virtual async Task<User?> DeleteAsync(string id, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new DeleteUserCommand(id), cancellationToken);
  }

  public virtual async Task<User?> ReadAsync(string? id, string? tenantId, string? uniqueName, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new ReadUserQuery(id, tenantId, uniqueName), cancellationToken);
  }

  public virtual async Task<CreatedToken?> RecoverPasswordAsync(RecoverPasswordPayload payload, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new RecoverPasswordCommand(payload), cancellationToken);
  }

  public virtual async Task<User?> ReplaceAsync(string id, ReplaceUserPayload payload, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new ReplaceUserCommand(id, payload), cancellationToken);
  }

  public virtual async Task<User?> ResetPasswordAsync(ResetPasswordPayload payload, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new ResetPasswordCommand(payload), cancellationToken);
  }

  public virtual async Task<SearchResults<User>> SearchAsync(SearchUserPayload payload, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new SearchUserQuery(payload), cancellationToken);
  }

  public virtual async Task<User?> SignOutAsync(string id, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new SignOutUserCommand(id), cancellationToken);
  }

  public virtual async Task<User?> UpdateAsync(string id, UpdateUserPayload payload, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new UpdateUserCommand(id, payload), cancellationToken);
  }
}
