using Logitar.Identity.Domain.Passwords;

namespace Logitar.Identity.Infrastructure;

public class PasswordConverter : JsonConverter<Password?>
{
  private readonly IPasswordService _passwordService;

  public PasswordConverter(IPasswordService passwordService)
  {
    _passwordService = passwordService;
  }

  public override Password? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? encoded = reader.GetString();

    return encoded == null ? null : _passwordService.Decode(encoded);
  }

  public override void Write(Utf8JsonWriter writer, Password? password, JsonSerializerOptions options)
  {
    writer.WriteStringValue(password?.Encode());
  }
}
