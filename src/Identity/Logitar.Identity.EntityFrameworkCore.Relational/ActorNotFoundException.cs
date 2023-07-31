namespace Logitar.Identity.EntityFrameworkCore.Relational;

public class ActorNotFoundException : Exception
{
  public ActorNotFoundException(string id) : base($"The actor 'Id={id}' could not be found.")
  {
    Id = id;
  }

  public string Id
  {
    get => (string)Data[nameof(Id)]!;
    private set => Data[nameof(Id)] = value;
  }
}
