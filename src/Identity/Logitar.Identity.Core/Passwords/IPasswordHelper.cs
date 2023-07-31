using Logitar.Security.Cryptography;

namespace Logitar.Identity.Core.Passwords;

public interface IPasswordHelper
{
  Password Create(string password);
  Password Decode(string encoded);
  Password Generate(int length, out byte[] password);
}
