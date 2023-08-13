namespace Logitar.Identity.Domain.Users;

public interface IUserManager
{
  Task DeleteAsync(UserAggregate user, CancellationToken cancellationToken = default);
  Task SaveAsync(UserAggregate user, CancellationToken cancellationToken = default);
}
