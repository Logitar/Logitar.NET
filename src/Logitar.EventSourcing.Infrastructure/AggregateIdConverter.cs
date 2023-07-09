using System.Text.Json;
using System.Text.Json.Serialization;

namespace Logitar.EventSourcing.Infrastructure;

public class AggregateIdConverter : JsonConverter<AggregateId>
{
  public override AggregateId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();

    return value == null ? default : new AggregateId(value);
  }

  public override void Write(Utf8JsonWriter writer, AggregateId aggregateId, JsonSerializerOptions options)
  {
    writer.WriteStringValue(aggregateId.Value);
  }
}
