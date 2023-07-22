namespace Logitar.Identity.Domain;

public record MayBe<T>
{
  public MayBe(T? value = default)
  {
    Value = value;
  }

  public T? Value { get; init; }
}
