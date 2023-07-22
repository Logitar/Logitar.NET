using Logitar.Security;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Converters;

public class Pbkdf2Converter : JsonConverter<Pbkdf2?>
{
  public override Pbkdf2? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? s = reader.GetString();

    return s == null ? null : Pbkdf2.Parse(s);
  }

  public override void Write(Utf8JsonWriter writer, Pbkdf2? pbkdf2, JsonSerializerOptions options)
  {
    writer.WriteStringValue(pbkdf2?.ToString());
  }
}
