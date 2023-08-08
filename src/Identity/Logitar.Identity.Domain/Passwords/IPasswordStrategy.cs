namespace Logitar.Identity.Domain.Passwords;

public interface IPasswordStrategy
{
  string Id { get; }

  Password Create(string password);
}
