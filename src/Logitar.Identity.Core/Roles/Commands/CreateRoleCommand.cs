using Logitar.Identity.Core.Roles.Models;
using MediatR;

namespace Logitar.Identity.Core.Roles.Commands;

/// <summary>
/// Represents the command used to create a role in the identity system.
/// <br />It returns a read model of the created role.
/// </summary>
/// <param name="Payload">The role creation data.</param>
public record CreateRoleCommand(CreateRolePayload Payload) : IRequest<Role>;
