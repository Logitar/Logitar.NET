namespace Logitar.Identity.Domain.Settings;

public record UniqueNameSettings : IUniqueNameSettings
{
  public string? AllowedCharacters { get; init; }
}
