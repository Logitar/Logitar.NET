namespace Logitar;

/// <summary>
/// Provides extension methods for <see cref="ICollection{T}"/> instances.
/// </summary>
public static class CollectionExtensions
{
  /// <summary>
  /// Adds the elements of the given collection to the end of the specified collection.
  /// </summary>
  /// <typeparam name="T">The type of the items to be added.</typeparam>
  /// <param name="collection">The collection to add items to.</param>
  /// <param name="items">The items to add to the collection.</param>
  public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
  {
    foreach (T item in items)
    {
      collection.Add(item);
    }
  }
}
