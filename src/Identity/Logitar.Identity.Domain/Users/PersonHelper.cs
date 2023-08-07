namespace Logitar.Identity.Domain.Users;

public static class PersonHelper
{
  public static string? BuildFullName(params string?[] names) => string.Join(' ', names
    .SelectMany(name => name?.Split() ?? Array.Empty<string>())
    .Where(name => !string.IsNullOrWhiteSpace(name))).CleanTrim();
}
