using Logitar.Security;

namespace Logitar.Identity.Domain.Passwords;

public static class PasswordHelper
{
  public static Password Decode(string encoded)
  {
    string kind = encoded.Split(Password.Separator).First();
    return kind switch
    {
      Pbkdf2.Prefix => Pbkdf2.Decode(encoded),
      _ => throw new NotSupportedException($"The password kind '{kind}' is not supported."),
    };
  }

  public static Password Generate(int length, out byte[] password)
  {
    password = RandomNumberGenerator.GetBytes(length);

    return new Pbkdf2(password);
  }
}
