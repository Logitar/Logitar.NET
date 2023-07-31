using Logitar.Identity.Core.Passwords;
using Logitar.Security.Cryptography;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Converters;

public class PasswordConverter : JsonConverter<Password?>
{
  public override Password? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? password = reader.GetString();

    return password == null ? null : PasswordHelper.Decode(password);
  }

  public override void Write(Utf8JsonWriter writer, Password? password, JsonSerializerOptions options)
  {
    writer.WriteStringValue(password?.Encode());
  }
}
