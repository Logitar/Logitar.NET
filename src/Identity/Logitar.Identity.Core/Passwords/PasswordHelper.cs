using FluentValidation;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users.Validators;
using Logitar.Security;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.Core.Passwords;

public class PasswordHelper : IPasswordHelper
{
  private readonly IOptions<UserSettings> _userSettings;

  public PasswordHelper(IOptions<UserSettings> userSettings)
  {
    _userSettings = userSettings;
  }

  public Password Create(string password)
  {
    UserSettings userSettings = _userSettings.Value;

    new PasswordValidator(userSettings.PasswordSettings, "Password").ValidateAndThrow(password);

    return new Pbkdf2(password);
  }

  public Password Decode(string encoded)
  {
    var kind = encoded.Split(Password.Separator).First();
    return kind switch
    {
      Pbkdf2.Prefix => Pbkdf2.Decode(encoded),
      _ => throw new NotSupportedException($"The password kind '{kind}' is not supported."),
    };
  }

  public Password Generate(int length, out byte[] password)
  {
    password = RandomNumberGenerator.GetBytes(length);

    return new Pbkdf2(password);
  }
}
