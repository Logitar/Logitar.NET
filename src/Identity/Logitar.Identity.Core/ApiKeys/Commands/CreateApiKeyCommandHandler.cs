using Logitar.Identity.Core.ApiKeys.Models;
using Logitar.Identity.Core.ApiKeys.Payloads;
using Logitar.Identity.Core.Passwords;
using Logitar.Identity.Core.Roles.Queries;
using Logitar.Identity.Domain.ApiKeys;
using Logitar.Identity.Domain.Roles;
using Logitar.Security;
using MediatR;

namespace Logitar.Identity.Core.ApiKeys.Commands;

public class CreateApiKeyCommandHandler : IRequestHandler<CreateApiKeyCommand, ApiKey>
{
  private readonly IApiKeyQuerier _apiKeyQuerier;
  private readonly IApiKeyRepository _apiKeyRepository;
  private readonly IMediator _mediator;
  private readonly IPasswordHelper _passwordHelper;

  public CreateApiKeyCommandHandler(IApiKeyQuerier apiKeyQuerier, IApiKeyRepository apiKeyRepository,
    IMediator mediator, IPasswordHelper passwordHelper)
  {
    _apiKeyQuerier = apiKeyQuerier;
    _apiKeyRepository = apiKeyRepository;
    _mediator = mediator;
    _passwordHelper = passwordHelper;
  }

  public async Task<ApiKey> Handle(CreateApiKeyCommand command, CancellationToken cancellationToken)
  {
    CreateApiKeyPayload payload = command.Payload;

    Password secret = _passwordHelper.Generate(ApiKeyAggregate.SecretLength, out byte[] secretBytes);
    ApiKeyAggregate apiKey = new(secret, payload.Title, payload.TenantId)
    {
      Description = payload.Description,
      ExpiresOn = payload.ExpiresOn
    };

    IEnumerable<RoleAggregate> roles = await _mediator.Send(new FindRolesQuery(apiKey.TenantId, payload.Roles, nameof(payload.Roles)), cancellationToken);
    foreach (RoleAggregate role in roles)
    {
      apiKey.AddRole(role);
    }

    await _apiKeyRepository.SaveAsync(apiKey, cancellationToken);

    ApiKey result = await _apiKeyQuerier.ReadAsync(apiKey, cancellationToken);
    result.Secret = secretBytes;

    return result;
  }
}
