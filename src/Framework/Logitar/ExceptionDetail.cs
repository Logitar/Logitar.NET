namespace Logitar;

/// <summary>
/// A serializable representation of an <see cref="Exception"/>.
/// </summary>
public record ExceptionDetail
{
  /// <summary>
  /// Gets or sets the type of the current exception.
  /// </summary>
  public string Type { get; set; } = string.Empty;
  /// <summary>
  /// Gets or sets a message that describes the current exception.
  /// </summary>
  public string Message { get; set; } = string.Empty;
  /// <summary>
  /// Gets or sets the <see cref="ExceptionDetail"/> instance that caused the current exception.
  /// </summary>
  public ExceptionDetail? InnerException { get; set; }

  /// <summary>
  /// Gets or sets HRESULT, a coded numerical value that is assigned to a specific exception.
  /// </summary>
  public int HResult { get; set; }
  /// <summary>
  /// Gets or sets a link to the help file associated with this exception.
  /// </summary>
  public string? HelpLink { get; set; }
  /// <summary>
  /// Gets or sets the name of the application or the object that causes the error.
  /// </summary>
  public string? Source { get; set; }
  /// <summary>
  /// Gets or sets a string representation of the immediate frames on the call stack.
  /// </summary>
  public string? StackTrace { get; set; }
  /// <summary>
  /// Gets or sets the method that throws the current exception.
  /// </summary>
  public string? TargetSite { get; set; }

  /// <summary>
  /// Gets or sets a collection of key/value pairs that provide additional user-defined information about the exception.
  /// </summary>
  public List<DictionaryEntry> Data { get; set; } = new();

  /// <summary>
  /// Creates a serializable representation of the specified exception.
  /// </summary>
  /// <param name="exception">The exception to represent.</param>
  /// <returns>The serializable representation.</returns>
  public static ExceptionDetail From(Exception exception)
  {
    ExceptionDetail detail = new()
    {
      Type = exception.GetType().GetLongestName(),
      Message = exception.Message,
      InnerException = exception.InnerException == null ? null : From(exception.InnerException),
      HResult = exception.HResult,
      HelpLink = exception.HelpLink,
      Source = exception.Source,
      StackTrace = exception.StackTrace,
      TargetSite = exception.TargetSite?.ToString()
    };

    foreach (DictionaryEntry entry in exception.Data)
    {
      if (IsSerializable(entry.Key) && (entry.Value == null || IsSerializable(entry.Value)))
      {
        detail.Data.Add(entry);
      }
    }

    return detail;
  }

  private static bool IsSerializable(object instance)
  {
    try
    {
      _ = JsonSerializer.Serialize(instance, instance.GetType());
      return true;
    }
    catch (Exception)
    {
      return false;
    }
  }
}
