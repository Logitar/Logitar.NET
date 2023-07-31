using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Sessions.Models;
using Logitar.Identity.Domain.Users;
using MediatR;

namespace Logitar.Identity.Core.Sessions.Commands;

public record SignInUserCommand : IRequest<Session>
{
  public SignInUserCommand(UserAggregate user, string? password = null, bool isPersistent = false,
    IEnumerable<CustomAttribute>? customAttributes = null)
  {
    User = user;
    Password = password;
    IsPersistent = isPersistent;
    CustomAttributes = customAttributes ?? Enumerable.Empty<CustomAttribute>();
  }

  public UserAggregate User { get; }
  public string? Password { get; }
  public bool IsPersistent { get; }
  public IEnumerable<CustomAttribute> CustomAttributes { get; }
}
