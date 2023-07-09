using System.Text.Json.Serialization;

namespace Logitar.EventSourcing.Infrastructure;

public interface IEventSerializer
{
  void RegisterConverter(JsonConverter converter);

  DomainEvent Deserialize(IEventEntity entity);
  string Serialize(DomainEvent change);
}
