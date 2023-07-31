using Logitar.Identity.Core.Models;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer;

public static class CustomAttributeExtensions
{
  public static IEnumerable<CustomAttribute> GetCustomAttributes(this ICustomAttributesProvider provider)
  {
    Dictionary<string, string> customAttributes = (provider.CustomAttributes == null ? null
      : JsonSerializer.Deserialize<Dictionary<string, string>>(provider.CustomAttributes))
      ?? new();

    return customAttributes.Select(customAttribute => new CustomAttribute(customAttribute.Key, customAttribute.Value));
  }

  public static string? UpdateCustomAttributes(this ICustomAttributesProvider provider, Dictionary<string, string?> updates)
  {
    Dictionary<string, string> customAttributes = (provider.CustomAttributes == null ? null
      : JsonSerializer.Deserialize<Dictionary<string, string>>(provider.CustomAttributes))
      ?? new();

    foreach (var (key, value) in updates)
    {
      if (value == null)
      {
        customAttributes.Remove(key);
      }
      else
      {
        customAttributes[key] = value;
      }
    }

    return customAttributes.Any() ? JsonSerializer.Serialize(customAttributes) : null;
  }
}
