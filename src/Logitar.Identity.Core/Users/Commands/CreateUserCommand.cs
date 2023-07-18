using Logitar.Identity.Core.Users.Models;
using MediatR;

namespace Logitar.Identity.Core.Users.Commands;

/// <summary>
/// Represents the command used to create an user in the identity system.
/// <br />It returns a read model of the created user.
/// </summary>
/// <param name="Payload">The user creation data.</param>
public record CreateUserCommand(CreateUserPayload Payload) : IRequest<User>;
