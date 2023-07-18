using Logitar.Identity.Core.ApiKeys.Models;
using MediatR;

namespace Logitar.Identity.Core.ApiKeys.Commands;

/// <summary>
/// Represents the command used to delete an API key in the identity system.
/// <br />It returns a read model of the deleted API key, or null if it was not found.
/// </summary>
/// <param name="Id">The identifier of the API key to delete.</param>
public record DeleteApiKeyCommand(string Id) : IRequest<ApiKey?>;
