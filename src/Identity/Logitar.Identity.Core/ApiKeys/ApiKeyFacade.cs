using Logitar.Identity.Core.ApiKeys.Commands;
using Logitar.Identity.Core.ApiKeys.Models;
using Logitar.Identity.Core.ApiKeys.Payloads;
using Logitar.Identity.Core.ApiKeys.Queries;
using Logitar.Identity.Core.Models;
using MediatR;

namespace Logitar.Identity.Core.ApiKeys;

public class ApiKeyFacade : IApiKeyFacade
{
  private readonly IMediator _mediator;

  public ApiKeyFacade(IMediator mediator)
  {
    _mediator = mediator;
  }

  public virtual async Task<ApiKey> AuthenticateAsync(AuthenticateApiKeyPayload payload, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new AuthenticateApiKeyCommand(payload), cancellationToken);
  }

  public virtual async Task<ApiKey> CreateAsync(CreateApiKeyPayload payload, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new CreateApiKeyCommand(payload), cancellationToken);
  }

  public virtual async Task<ApiKey?> DeleteAsync(string id, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new DeleteApiKeyCommand(id), cancellationToken);
  }

  public virtual async Task<ApiKey?> ReadAsync(string? id, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new ReadApiKeyQuery(id), cancellationToken);
  }

  public virtual async Task<ApiKey?> ReplaceAsync(string id, ReplaceApiKeyPayload payload, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new ReplaceApiKeyCommand(id, payload), cancellationToken);
  }

  public virtual async Task<SearchResults<ApiKey>> SearchAsync(SearchApiKeyPayload payload, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new SearchApiKeyQuery(payload), cancellationToken);
  }

  public virtual async Task<ApiKey?> UpdateAsync(string id, UpdateApiKeyPayload payload, CancellationToken cancellationToken)
  {
    return await _mediator.Send(new UpdateApiKeyCommand(id, payload), cancellationToken);
  }
}
