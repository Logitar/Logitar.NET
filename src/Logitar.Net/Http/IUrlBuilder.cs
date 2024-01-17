namespace Logitar.Net.Http;

public interface IUrlBuilder
{
  /// <summary>
  /// Gets the Uniform Resource Locator (URL) parameters.
  /// </summary>
  IReadOnlyDictionary<string, string> Parameters { get; }
  /// <summary>
  /// Gets the Uniform Resource Locator (URL) query parameters.
  /// </summary>
  IReadOnlyDictionary<string, IEnumerable<string>> Query { get; }
  /// <summary>
  /// Gets the query string.
  /// </summary>
  string QueryString { get; }

  /// <summary>
  /// Sets an Uniform Resource Locator (URL) parameter. It can contain placeholders using the format '{key}' that will be replaced by parameter values.
  /// </summary>
  /// <param name="key">The key of the parameter.</param>
  /// <param name="value">The value of the parameter.</param>
  /// <returns>The builder instance.</returns>
  IUrlBuilder SetParameter(string key, string value);

  /// <summary>
  /// Adds an Uniform Resource Locator (URL) query parameter.
  /// </summary>
  /// <param name="key">The key of the parameter.</param>
  /// <param name="value">The value of the parameter.</param>
  /// <returns>The builder instance.</returns>
  IUrlBuilder AddQuery(string key, string value);
  /// <summary>
  /// Adds an Uniform Resource Locator (URL) query parameter.
  /// </summary>
  /// <param name="key">The key of the parameter.</param>
  /// <param name="values">The values of the parameter.</param>
  /// <returns>The builder instance.</returns>
  IUrlBuilder AddQuery(string key, IEnumerable<string> values);
  /// <summary>
  /// Sets an Uniform Resource Locator (URL) query parameter.
  /// </summary>
  /// <param name="key">The key of the parameter.</param>
  /// <param name="value">The value of the parameter.</param>
  /// <returns>The builder instance.</returns>
  IUrlBuilder SetQuery(string key, string value);
  /// <summary>
  /// Sets an Uniform Resource Locator (URL) query parameter.
  /// </summary>
  /// <param name="key">The key of the parameter.</param>
  /// <param name="values">The values of the parameter.</param>
  /// <returns>The builder instance.</returns>
  IUrlBuilder SetQuery(string key, IEnumerable<string> values);

  /// <summary>
  /// Builds the request Uniform Resource Identifier (URI).
  /// </summary>
  /// <returns>The built instance.</returns>
  Uri BuildUri();
}
