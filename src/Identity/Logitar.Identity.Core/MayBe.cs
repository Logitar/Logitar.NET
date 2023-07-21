namespace Logitar.Identity.Core;

public record MayBe<T>
{
  public MayBe(T? value = default)
  {
    Value = value;
  }

  public T? Value { get; init; }
}
