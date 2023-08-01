namespace Logitar.Identity.Core.Users;

public class ExternalIdentifierAlreadyUsedException : Exception
{
  public const string ErrorMessage = "The specified external identifier is already used.";

  public ExternalIdentifierAlreadyUsedException(string? tenantId, string key, string value)
    : base(BuildMessage(tenantId, key, value))
  {
    TenantId = tenantId;
    Key = key;
    Value = value;
  }

  public string? TenantId
  {
    get => (string?)Data[nameof(TenantId)];
    private set => Data[nameof(TenantId)] = value;
  }
  public string Key
  {
    get => (string)Data[nameof(Key)]!;
    private set => Data[nameof(Key)] = value;
  }
  public string Value
  {
    get => (string)Data[nameof(Value)]!;
    private set => Data[nameof(Value)] = value;
  }

  private static string BuildMessage(string? tenantId, string key, string value)
  {
    StringBuilder message = new();

    message.AppendLine(ErrorMessage);
    message.Append("TenantId: ").AppendLine(tenantId);
    message.Append("Key: ").AppendLine(key);
    message.Append("Value: ").AppendLine(value);

    return message.ToString();
  }
}
