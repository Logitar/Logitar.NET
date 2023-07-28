using Logitar.Identity.Core.Tokens.Commands;
using Logitar.Identity.Core.Tokens.Models;
using Logitar.Identity.Core.Tokens.Payloads;
using MediatR;

namespace Logitar.Identity.Core.Tokens;

public class TokenFacade : ITokenFacade
{
  private readonly IMediator _mediator;

  public TokenFacade(IMediator mediator)
  {
    _mediator = mediator;
  }

  public virtual async Task<CreatedToken> CreateAsync(CreateTokenPayload payload, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new CreateTokenCommand(payload), cancellationToken);
  }

  public virtual async Task<ValidatedToken> ValidateAsync(ValidateTokenPayload payload, bool consume, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new ValidateTokenCommand(payload, consume), cancellationToken);
  }
}
