namespace Logitar.Identity.Domain.Passwords;

public interface IPasswordService
{
  Password Create(string password);
}
