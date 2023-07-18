using Logitar.Identity.Core.ApiKeys.Models;
using MediatR;

namespace Logitar.Identity.Core.ApiKeys.Commands;

/// <summary>
/// Represents the command used to create an API key in the identity system.
/// <br />It returns a read model of the created API key.
/// </summary>
/// <param name="Payload">The API key creation data.</param>
public record CreateApiKeyCommand(CreateApiKeyPayload Payload) : IRequest<ApiKey>;
