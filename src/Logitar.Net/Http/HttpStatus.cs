namespace Logitar.Net.Http;

/// <summary>
/// Represents the status of an HTTP response.
/// </summary>
public record HttpStatus
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
  /// Gets or sets a value indicating whether or not the status is a success.
  /// </summary>
  public bool IsSuccess { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpStatus"/> class.
  /// </summary>
  public HttpStatus()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpStatus"/> class.
  /// </summary>
  /// <param name="status">The HTTP status code.</param>
  public HttpStatus(HttpStatusCode status)
  {
    Code = (int)status;
    Text = status.ToString();
    Value = status;
    IsSuccess = Code >= 200 && Code <= 299;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpStatus"/> class.
  /// </summary>
  /// <param name="response">The HTTP response message.</param>
  public HttpStatus(HttpResponseMessage response) : this(response.StatusCode)
  {
    IsSuccess = response.IsSuccessStatusCode;
  }
}
