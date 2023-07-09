namespace Logitar;

/// <summary>
/// TODO(fpion): document
/// </summary>
public static class CollectionExtensions
{
  /// <summary>
  /// TODO(fpion): document
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="collection"></param>
  /// <param name="items"></param>
  public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
  {
    foreach (T item in items)
    {
      collection.Add(item);
    }
  }
}
