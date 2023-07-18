using Logitar.Identity.Core.Users.Models;
using MediatR;

namespace Logitar.Identity.Core.Users.Commands;

/// <summary>
/// Represents the command used to delete an user in the identity system.
/// <br />It returns a read model of the deleted user, or null if it was not found.
/// </summary>
/// <param name="Id">The identifier of the user to delete.</param>
public record DeleteUserCommand(string Id) : IRequest<User?>;
