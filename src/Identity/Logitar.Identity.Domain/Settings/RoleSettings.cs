namespace Logitar.Identity.Domain.Settings;

public record RoleSettings : IRoleSettings
{

  public IUniqueNameSettings UniqueNameSettings { get; set; } = new UniqueNameSettings
  {
    AllowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_"
  };
}
