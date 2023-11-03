namespace Logitar;

[Trait(Traits.Category, Categories.Unit)]
public class TypeExtensionsTests
{
  private const string Namespace = nameof(Logitar);
  private const string TypeName = nameof(TypeMock);

  private readonly Assembly _assembly = Assembly.GetExecutingAssembly();

  [Fact(DisplayName = "GetLongestName: it should use the AssemblyQualifiedName when it is available.")]
  public void GetLongestName_it_should_use_the_AssemblyQualifiedName_when_it_is_available()
  {
    TypeMock type = new(_assembly, TypeName, Namespace);
    Assert.Equal(type.AssemblyQualifiedName, type.GetLongestName());
  }

  [Fact(DisplayName = "GetLongestName: it should use the FullName when it is available.")]
  public void GetLongestName_it_should_use_the_FullName_when_it_is_available()
  {
    AssemblyMock assembly = new(name: null);
    TypeMock type = new(assembly, TypeName, Namespace);
    Assert.Equal(type.FullName, type.GetLongestName());
  }

  [Fact(DisplayName = "GetLongestName: it should use the Name when others are not available.")]
  public void GetLongestName_it_should_use_the_Name_when_others_are_not_available()
  {
    AssemblyMock assembly = new(name: null);
    TypeMock type = new(assembly, TypeName, @namespace: null);
    Assert.Equal(type.Name, type.GetLongestName());
  }

  [Fact(DisplayName = "GetNamespaceQualifiedName: it returns the correct type name.")]
  public void GetNamespaceQualifiedName_it_returns_the_correct_type_name()
  {
    TypeMock type = new(_assembly, TypeName, Namespace);
    Assert.Equal($"{Namespace}.{TypeName}, {_assembly.GetName().Name}", type.GetNamespaceQualifiedName());
  }

  [Fact(DisplayName = "GetNamespaceQualifiedName: it throws ArgumentException when assembly name is null.")]
  public void GetNamespaceQualifiedName_it_throws_ArgumentException_when_assembly_name_is_null()
  {
    AssemblyMock assembly = new(name: null);
    TypeMock type = new(assembly, TypeName, Namespace);
    var exception = Assert.Throws<ArgumentException>(type.GetNamespaceQualifiedName);
    Assert.StartsWith("The assembly simple name is required.", exception.Message);
    Assert.Equal("type", exception.ParamName);
  }

  [Fact(DisplayName = "GetNamespaceQualifiedName: it throws ArgumentException when FullName is null.")]
  public void GetNamespaceQualifiedName_it_throws_ArgumentException_when_FullName_is_null()
  {
    TypeMock type = new(_assembly, TypeName);
    var exception = Assert.Throws<ArgumentException>(type.GetNamespaceQualifiedName);
    Assert.StartsWith("The FullName is required.", exception.Message);
    Assert.Equal("type", exception.ParamName);
  }
}

