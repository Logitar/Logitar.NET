﻿namespace Logitar.EventSourcing;

/// <summary>
/// Provides extension methods for <see cref="Type"/> instances.
/// </summary>
public static class TypeExtensions
{
  /// <summary>
  /// Builds an unversioned, assembly qualified type name.
  /// </summary>
  /// <param name="type">The type to build its name.</param>
  /// <returns>The unversioned, assembly qualified type name.</returns>
  /// <exception cref="ArgumentException">The full name or assembly nameof of the type are null.</exception>
  public static string GetName(this Type type)
  {
    string typeName = type.FullName
      ?? throw new ArgumentException($"The {nameof(type.FullName)} is required.", nameof(type));

    string assemblyName = type.Assembly.GetName().Name
      ?? throw new ArgumentException("The assembly simple name is required.", nameof(type));

    return string.Join(", ", typeName, assemblyName);
  }
}
