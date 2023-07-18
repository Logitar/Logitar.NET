using Logitar.Identity.Core.Roles.Models;
using MediatR;

namespace Logitar.Identity.Core.Roles.Commands;

/// <summary>
/// Represents the command used to delete a role in the identity system.
/// <br />It returns a read model of the deleted role, or null if it was not found.
/// </summary>
/// <param name="Id">The identifier of the role to delete.</param>
public record DeleteRoleCommand(string Id) : IRequest<Role?>;
