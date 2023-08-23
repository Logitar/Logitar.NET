using FluentValidation;
using Logitar.Identity.Domain.Passwords.Validators;
using Logitar.Identity.Domain.Settings;

namespace Logitar.Identity.Domain.Passwords;

public class PasswordService : IPasswordService
{
  private readonly ISettingsResolver _settingsResolver;
  private readonly Dictionary<string, IPasswordStrategy> _strategies = new();

  public PasswordService(ISettingsResolver settingsResolver, IEnumerable<IPasswordStrategy> strategies)
  {
    _settingsResolver = settingsResolver;
    _strategies = strategies.GroupBy(x => x.Id).ToDictionary(g => g.Key, g => g.Last());
  }

  protected IPasswordSettings PasswordSettings => _settingsResolver.UserSettings.PasswordSettings;

  public Password Create(string password)
  {
    new PasswordValidator(PasswordSettings, "Password").ValidateAndThrow(password);

    if (!_strategies.TryGetValue(PasswordSettings.Strategy, out IPasswordStrategy? strategy))
    {
      throw new PasswordStrategyNotSupportedException(PasswordSettings.Strategy);
    }

    return strategy.Create(password);
  }

  public Password Decode(string encoded)
  {
    string strategyKey = encoded.Split(Password.Separator).First();
    if (!_strategies.TryGetValue(strategyKey, out IPasswordStrategy? strategy))
    {
      throw new PasswordStrategyNotSupportedException(PasswordSettings.Strategy);
    }

    return strategy.Decode(encoded);
  }

  public Password Generate(int length, out byte[] password)
  {
    if (!_strategies.TryGetValue(PasswordSettings.Strategy, out IPasswordStrategy? strategy))
    {
      throw new PasswordStrategyNotSupportedException(PasswordSettings.Strategy);
    }

    password = RandomNumberGenerator.GetBytes(length);

    return strategy.Create(Convert.ToBase64String(password));
  }
}
