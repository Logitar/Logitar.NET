namespace Logitar.Net.Http;

/// <summary>
/// Represents an HTTP header.
/// </summary>
public record HttpHeader
{
  /// <summary>
  /// Gets or sets the name of the header.
  /// </summary>
  public string Name { get; set; }
  /// <summary>
  /// Gets or sets the values of the header.
  /// </summary>
  public List<string> Values { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpHeader"/> class.
  /// </summary>
  public HttpHeader() : this(string.Empty)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpHeader"/> class.
  /// </summary>
  /// <param name="name">The name of the header.</param>
  public HttpHeader(string name)
  {
    Name = name;
    Values = [];
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpHeader"/> class.
  /// </summary>
  /// <param name="name">The name of the header.</param>
  /// <param name="value">The value of the header.</param>
  public HttpHeader(string name, string value) : this(name)
  {
    Values.Add(value);
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpHeader"/> class.
  /// </summary>
  /// <param name="name">The name of the header.</param>
  /// <param name="values">The values of the header.</param>
  public HttpHeader(string name, IEnumerable<string> values) : this(name)
  {
    Values.AddRange(values);
  }
}
