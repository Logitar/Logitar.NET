using System.Text.Json.Serialization;

namespace Logitar.EventSourcing.Infrastructure;

public interface IEventSerializer
{
  void RegisterConverter(JsonConverter converter);

  DomainEvent Deserialize(EventEntity entity);
  string Serialize(DomainEvent change);
}
