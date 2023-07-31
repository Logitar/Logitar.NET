using Logitar.EventSourcing;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

public class EntityNotFoundException : Exception
{
  public EntityNotFoundException(Type type, string id) : base(BuildMessage(type, id))
  {
    if (!type.IsSubclassOf(typeof(AggregateEntity)))
    {
      throw new ArgumentException($"The type must be a subclass of '{nameof(AggregateEntity)}'.", nameof(type));
    }

    Type = type.GetName();
    Id = id;
  }

  public string Type
  {
    get => (string)Data[nameof(Type)]!;
    private set => Data[nameof(Type)] = value;
  }
  public string Id
  {
    get => (string)Data[nameof(Id)]!;
    private set => Data[nameof(Id)] = value;
  }

  private static string BuildMessage(Type type, string id)
  {
    StringBuilder message = new();

    message.AppendLine("The specified entity could not be found.");
    message.Append("Type: ").AppendLine(type.GetName());
    message.Append("Id: ").AppendLine(id);

    return message.ToString();
  }
}

public class EntityNotFoundException<T> : EntityNotFoundException where T : AggregateEntity
{
  public EntityNotFoundException(string id) : base(typeof(T), id)
  {
  }
}
