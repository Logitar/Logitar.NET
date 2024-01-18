namespace Logitar.Net.Http;

/// <summary>
/// Defines a builder for Uniform Resource Locators (URL).
/// </summary>
public interface IUrlBuilder
{
  /// <summary>
  /// Gets the scheme of the URL.
  /// </summary>
  string Scheme { get; }

  /// <summary>
  /// Gets the user information (username/password) of the URL.
  /// </summary>
  ICredentials? Credentials { get; }
  /// <summary>
  /// Gets the host of the URL.
  /// </summary>
  string Host { get; }
  /// <summary>
  /// Gets the port of the URL.
  /// </summary>
  ushort Port { get; }
  /// <summary>
  /// Gets the authority of the URL.
  /// </summary>
  string Authority { get; }

  /// <summary>
  /// Gets the path segments of the URL.
  /// </summary>
  IReadOnlyCollection<string> Segments { get; }
  /// <summary>
  /// Gets the path of the URL.
  /// </summary>
  string? Path { get; }

  /// <summary>
  /// Gets the query parameters of the URL.
  /// </summary>
  IReadOnlyDictionary<string, IReadOnlyCollection<string>> Query { get; }
  /// <summary>
  /// Gets the query string of the URL.
  /// </summary>
  string? QueryString { get; }

  /// <summary>
  /// Gets the fragment of the URL.
  /// </summary>
  string? Fragment { get; }

  /// <summary>
  /// Gets the parameters of the URL. Parameters are replaced from placeholders using the format '{key}'.
  /// </summary>
  IReadOnlyDictionary<string, string> Parameters { get; }

  /// <summary>
  /// Sets the scheme of the URL.
  /// </summary>
  /// <param name="scheme">The URL scheme.</param>
  /// <param name="inferPort">A value indicating whether or not to infer the URL port from the specified scheme.</param>
  /// <returns>The URL builder.</returns>
  IUrlBuilder SetScheme(string scheme, bool inferPort = false);

  /// <summary>
  /// Sets the user information (username/password) of the URL.
  /// </summary>
  /// <param name="credentials">The user information.</param>
  /// <returns>The URL builder.</returns>
  IUrlBuilder SetCredentials(ICredentials? credentials);
  /// <summary>
  /// Sets the host of the URL.
  /// </summary>
  /// <param name="host">The URL host.</param>
  /// <returns>The URL builder.</returns>
  IUrlBuilder SetHost(string host);
  /// <summary>
  /// Sets the port of the URL.
  /// </summary>
  /// <param name="port">The URL port.</param>
  /// <returns>The URL builder.</returns>
  IUrlBuilder SetPort(ushort port);
  /// <summary>
  /// Sets the authority of the URL.
  /// </summary>
  /// <param name="authority">The authority of the URL.</param>
  /// <returns>The URL builder.</returns>
  IUrlBuilder SetAuthority(string authority);

  /// <summary>
  /// Sets the path segments of the URL.
  /// </summary>
  /// <param name="segments">The path segments.</param>
  /// <returns>The URL builder.</returns>
  IUrlBuilder SetSegments(IEnumerable<string> segments);
  /// <summary>
  /// Sets the path of the URL.
  /// </summary>
  /// <param name="path">The path of the URL.</param>
  /// <returns>The URL builder.</returns>
  IUrlBuilder SetPath(string? path);

  /// <summary>
  /// Appends a query parameter value.
  /// </summary>
  /// <param name="key">The key of the parameter.</param>
  /// <param name="value">The value of the parameter.</param>
  /// <returns>The URL builder.</returns>
  IUrlBuilder AddQuery(string key, string value);
  /// <summary>
  /// Appends a list of query parameter values.
  /// </summary>
  /// <param name="key">The key of the parameter.</param>
  /// <param name="values">The values of the parameter.</param>
  /// <returns>The URL builder.</returns>
  IUrlBuilder AddQuery(string key, IEnumerable<string> values);
  /// <summary>
  /// Sets the value of a query parameter.
  /// </summary>
  /// <param name="key">The key of the parameter.</param>
  /// <param name="value">The value of the parameter.</param>
  /// <returns>The URL builder.</returns>
  IUrlBuilder SetQuery(string key, string value);
  /// <summary>
  /// Sets the values of a query parameter.
  /// </summary>
  /// <param name="key">The key of the parameter.</param>
  /// <param name="values">The values of the parameter.</param>
  /// <returns>The URL builder.</returns>
  IUrlBuilder SetQuery(string key, IEnumerable<string> values);
  /// <summary>
  /// Sets the query string of the URL.
  /// </summary>
  /// <param name="queryString">The query string.</param>
  /// <returns>The URL builder.</returns>
  IUrlBuilder SetQueryString(string? queryString);

  /// <summary>
  /// Sets the fragment of the URL.
  /// </summary>
  /// <param name="fragment">The URL fragment.</param>
  /// <returns>The URL builder.</returns>
  IUrlBuilder SetFragment(string fragment);

  /// <summary>
  /// Sets an URL parameter.
  /// </summary>
  /// <param name="key">The key of the parameter.</param>
  /// <param name="value">The value of the parameter.</param>
  /// <returns>The URL builder.</returns>
  IUrlBuilder SetParameter(string key, string? value);

  /// <summary>
  /// Builds the URL.
  /// </summary>
  /// <param name="kind">The kind of URL to build.</param>
  /// <returns>The built URL.</returns>
  string Build(UriKind kind = UriKind.Absolute);
  /// <summary>
  /// Builds an Uniform Resource Identifier (URI) object.
  /// </summary>
  /// <param name="kind">The kind of URL to build.</param>
  /// <returns>The built instance.</returns>
  Uri BuildUri(UriKind kind = UriKind.Absolute);
}
