namespace Logitar;

internal class AssemblyMock : Assembly
{
  public string? Name { get; }

  public AssemblyMock(string? name)
  {
    Name = name;
  }

  public override AssemblyName GetName() => Name == null ? new AssemblyName() : new AssemblyName(Name);
}
