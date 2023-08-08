using FluentValidation;
using Logitar.Identity.Domain.Passwords.Validators;
using Logitar.Identity.Domain.Settings;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.Domain.Passwords;

public class PasswordService : IPasswordService
{
  private readonly IOptions<PasswordSettings> _passwordSettings;
  private readonly Dictionary<string, IPasswordStrategy> _strategies = new();

  public PasswordService(IOptions<PasswordSettings> passwordSettings,
    IEnumerable<IPasswordStrategy> strategies)
  {
    _passwordSettings = passwordSettings;
    _strategies = strategies.GroupBy(x => x.Id).ToDictionary(g => g.Key, g => g.Last());
  }

  public Password Create(string password)
  {
    PasswordSettings passwordSettings = _passwordSettings.Value;
    new PasswordValidator(passwordSettings, "Password").ValidateAndThrow(password);

    if (!_strategies.TryGetValue(passwordSettings.Strategy, out IPasswordStrategy? strategy))
    {
      throw new PasswordStrategyNotSupportedException(passwordSettings.Strategy);
    }

    return strategy.Create(password);
  }
}
