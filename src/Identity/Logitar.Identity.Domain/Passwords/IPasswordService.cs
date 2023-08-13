namespace Logitar.Identity.Domain.Passwords;

public interface IPasswordService
{
  Password Create(string password);
  Password Decode(string encoded);
  Password Generate(int length, out byte[] password);
}
