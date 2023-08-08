namespace Logitar.Identity.Domain.Settings;

public class PasswordStrategyNotSupportedException : Exception
{
  public PasswordStrategyNotSupportedException(string strategyId)
    : base($"The password strategy '{strategyId}' is not supported.")
  {
    StrategyId = strategyId;
  }

  public string StrategyId
  {
    get => (string)Data[nameof(StrategyId)]!;
    private set => Data[nameof(StrategyId)] = value;
  }
}
