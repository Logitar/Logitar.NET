using FluentValidation;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users.Validators;
using Logitar.Security.Cryptography;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.Core.Passwords;

public class PasswordHelper : IPasswordHelper
{
  private readonly IOptions<Pbkdf2Settings> _pbkdf2Settings;
  private readonly IOptions<UserSettings> _userSettings;

  public PasswordHelper(IOptions<Pbkdf2Settings> pbkdf2Settings, IOptions<UserSettings> userSettings)
  {
    _pbkdf2Settings = pbkdf2Settings;
    _userSettings = userSettings;
  }

  public Password Create(string password)
  {
    UserSettings userSettings = _userSettings.Value;
    new PasswordValidator(userSettings.PasswordSettings, "Password").ValidateAndThrow(password);

    Pbkdf2Settings pbkdf2Settings = _pbkdf2Settings.Value;
    new Pbkdf2Validator().ValidateAndThrow(pbkdf2Settings);
    return new Pbkdf2(password, pbkdf2Settings);
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

    Pbkdf2Settings pbkdf2Settings = _pbkdf2Settings.Value;
    new Pbkdf2Validator().ValidateAndThrow(pbkdf2Settings);
    return new Pbkdf2(password, pbkdf2Settings);
  }
}
