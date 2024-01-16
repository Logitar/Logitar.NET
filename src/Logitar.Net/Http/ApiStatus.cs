namespace Logitar.Net.Http;

/// <summary>
/// Represents the status of an API call.
/// </summary>
public record ApiStatus
{
  /// <summary>
  /// Gets or sets the status code.
  /// </summary>
  public int Code { get; set; }

  /// <summary>
  /// Gets or sets the status text.
  /// </summary>
  public string Text { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the status value.
  /// </summary>
  [JsonIgnore]
  public HttpStatusCode Value { get; set; }

  /// <summary>
  /// Gets or sets a value indicating whether the status is a success or a failure.
  /// </summary>
  public bool IsSuccess { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiStatus"/> class.
  /// </summary>
  public ApiStatus()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiStatus"/> class.
  /// </summary>
  /// <param name="statusCode">The HTTP status code.</param>
  public ApiStatus(HttpStatusCode statusCode) : this(statusCode, isSuccess: false)
  {
    IsSuccess = Code >= 200 && Code <= 299;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiStatus"/> class.
  /// </summary>
  /// <param name="statusCode">The HTTP status code.</param>
  /// <param name="isSuccess">A value indicating whether the status is a success or a failure.</param>
  public ApiStatus(HttpStatusCode statusCode, bool isSuccess)
  {
    Code = (int)statusCode;
    Text = statusCode.ToString();
    Value = statusCode;
    IsSuccess = isSuccess;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiStatus"/> class.
  /// </summary>
  /// <param name="response">The HTTP response message.</param>
  public ApiStatus(HttpResponseMessage response) : this(response.StatusCode, response.IsSuccessStatusCode)
  {
  }
}
