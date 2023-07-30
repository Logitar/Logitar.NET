using Logitar.Security;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Converters;

public class Pbkdf2Converter : JsonConverter<Pbkdf2?>
{
  public override Pbkdf2? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? pbkdf2 = reader.GetString();

    return pbkdf2 == null ? null : Pbkdf2.Decode(pbkdf2);
  }

  public override void Write(Utf8JsonWriter writer, Pbkdf2? pbkdf2, JsonSerializerOptions options)
  {
    writer.WriteStringValue(pbkdf2?.Encode());
  }
}
