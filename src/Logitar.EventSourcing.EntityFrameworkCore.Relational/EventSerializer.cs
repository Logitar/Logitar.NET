using System.Text.Json;
using System.Text.Json.Serialization;

namespace Logitar.EventSourcing.EntityFrameworkCore.Relational;

/// <summary>
/// TODO(fpion): document
/// </summary>
public class EventSerializer
{
  private static readonly object _lock = new();
  private static EventSerializer? _instance = null;
  public static EventSerializer Instance
  {
    get
    {
      lock (_lock)
      {
        _instance ??= new();
        return _instance;
      }
    }
  }

  private readonly JsonSerializerOptions _options = new();

  private EventSerializer()
  {
    RegisterConverter(new AggregateIdConverter());
    RegisterConverter(new JsonStringEnumConverter());
  }

  public void RegisterConverter(JsonConverter converter)
  {
    _options.Converters.Add(converter);
  }

  public DomainEvent Deserialize(EventEntity entity)
  {
    Type eventType = Type.GetType(entity.EventType) ?? throw new EventTypeNotFoundException(entity);

    return (DomainEvent?)JsonSerializer.Deserialize(entity.EventData, eventType, _options)
      ?? throw new EventDataDeserializationFailedException(entity);
  }

  public string Serialize(DomainEvent change)
  {
    return JsonSerializer.Serialize(change, change.GetType(), _options);
  }
}
