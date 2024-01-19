namespace Logitar.Net.Mail;

/// <summary>
/// Represents the result of an email sending operation.
/// </summary>
public record SendMailResult
{
  /// <summary>
  /// Gets a value indicating whether or not the operation succeeded.
  /// </summary>
  public bool Succeeded { get; }

  /// <summary>
  /// Gets the result data.
  /// </summary>
  public IReadOnlyDictionary<string, object?> Data { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="SendMailResult"/> class.
  /// </summary>
  /// <param name="succeeded">A value indicating whether or not the operation succeeded.</param>
  /// <param name="data">The result data.</param>
  public SendMailResult(bool succeeded, IDictionary<string, object?>? data = null)
  {
    Succeeded = succeeded;
    Data = (data ?? new Dictionary<string, object?>()).AsReadOnly();
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="SendMailResult"/> class with a failure status.
  /// </summary>
  /// <param name="data">The result data.</param>
  public static SendMailResult Failure(IDictionary<string, object?>? data = null) => new(succeeded: false, data);

  /// <summary>
  /// Initializes a new instance of the <see cref="SendMailResult"/> class with a success status.
  /// </summary>
  /// <param name="data">The result data.</param>
  public static SendMailResult Success(IDictionary<string, object?>? data = null) => new(succeeded: true, data);
}
