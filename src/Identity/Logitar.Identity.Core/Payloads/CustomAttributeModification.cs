namespace Logitar.Identity.Core.Payloads;

public record CustomAttributeModification
{
  public CustomAttributeModification() : this(string.Empty, null)
  {
  }
  public CustomAttributeModification(string key, string? value)
  {
    Key = key;
    Value = value;
  }

  public string Key { get; set; }
  public string? Value { get; set; }
}
