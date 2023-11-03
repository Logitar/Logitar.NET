namespace Logitar;

/// <summary>
/// Provides extension methods for <see cref="Type"/> instances.
/// </summary>
public static class TypeExtensions
{
  /// <summary>
  /// Returns the longest available name of the specified type, which is the assembly-qualified name, followed by the full name and finally the name.
  /// </summary>
  /// <param name="type">The type.</param>
  /// <returns>The longest available name of the type.</returns>
  public static string GetLongestName(this Type type)
  {
    return type.AssemblyQualifiedName ?? type.FullName ?? type.Name;
  }

  /// <summary>
  /// Returns the namespace-qualified name of the specified type, which includes the namespace from which this <see cref="Type"/> object was loaded.
  /// </summary>
  /// <param name="type">The type.</param>
  /// <returns>The namespace-qualified name of the type.</returns>
  public static string GetNamespaceQualifiedName(this Type type)
  {
    string typeName = type.FullName
      ?? throw new ArgumentException($"The {nameof(type.FullName)} is required.", nameof(type));

    string assemblyName = type.Assembly.GetName().Name
      ?? throw new ArgumentException("The assembly simple name is required.", nameof(type));

    return string.Join(", ", typeName, assemblyName);
  }
}
