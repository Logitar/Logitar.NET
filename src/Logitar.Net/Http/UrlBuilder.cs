namespace Logitar.Net.Http;

/// <summary>
/// Implements a builder for Uniform Resource Locators (URL).
/// </summary>
public class UrlBuilder : IUrlBuilder
{
  /// <summary>
  /// The default URL scheme.
  /// </summary>
  public const string DefaultScheme = "http";
  /// <summary>
  /// The default URL host.
  /// </summary>
  public const string DefaultHost = "localhost";

  /// <summary>
  /// Gets a list of supported URL schemes.
  /// </summary>
  public static IReadOnlyCollection<string> SupportedSchemes => _supportedSchemes.ToList().AsReadOnly();
  /// <summary>
  /// A hash set of supported URL schemes.
  /// </summary>
  protected static readonly HashSet<string> _supportedSchemes = ["http", "https"];
  /// <summary>
  /// Returns a value indicating whether or not the specified scheme is supported.
  /// </summary>
  /// <param name="scheme">The scheme to verify.</param>
  /// <returns>True if the scheme is supported, false otherwise.</returns>
  public static bool IsSchemeSupported(string scheme) => _supportedSchemes.Contains(scheme.Trim().ToLower());

  /// <summary>
  /// Gets or sets the scheme of the URL.
  /// </summary>
  public virtual string Scheme { get; protected set; }

  /// <summary>
  /// Gets the user information (username/password) of the URL.
  /// </summary>
  public virtual ICredentials? Credentials { get; protected set; }
  /// <summary>
  /// Gets or sets the host of the URL.
  /// </summary>
  public virtual string Host { get; protected set; }
  /// <summary>
  /// Gets or sets the port of the URL.
  /// </summary>
  public virtual ushort Port { get; protected set; }
  /// <summary>
  /// Gets the authority of the URL.
  /// </summary>
  public virtual string Authority
  {
    get
    {
      StringBuilder authority = new();
      if (Credentials != null)
      {
        authority.Append(Credentials.Identifier).Append(':').Append(Credentials.Secret).Append('@');
      }
      authority.Append(Host).Append(':').Append(Port);
      return authority.ToString();
    }
  }

  /// <summary>
  /// Gets the path segments of the URL.
  /// </summary>
  public virtual IReadOnlyCollection<string> Segments => UrlSegments.AsReadOnly();
  /// <summary>
  /// Gets or sets a list of URL segments.
  /// </summary>
  protected virtual List<string> UrlSegments { get; set; } = [];
  /// <summary>
  /// Gets the path of the URL.
  /// </summary>
  public virtual string? Path => UrlSegments.Count == 0 ? null : string.Join('/', UrlSegments);

  /// <summary>
  /// Gets the query parameters of the URL.
  /// </summary>
  public virtual IReadOnlyDictionary<string, IReadOnlyCollection<string>> Query
  {
    get
    {
      Dictionary<string, IReadOnlyCollection<string>> query = [];
      foreach (KeyValuePair<string, List<string>> parameter in QueryParameters)
      {
        query[parameter.Key] = parameter.Value.AsReadOnly();
      }
      return query.AsReadOnly();
    }
  }
  /// <summary>
  /// Gets or sets the query parameters of the URL.
  /// </summary>
  protected virtual Dictionary<string, List<string>> QueryParameters { get; set; } = [];
  /// <summary>
  /// Gets the query string of the URL.
  /// </summary>
  public virtual string? QueryString
  {
    get
    {
      if (QueryParameters.Count == 0)
      {
        return null;
      }

      List<string> parameters = [];
      foreach (KeyValuePair<string, List<string>> parameter in QueryParameters)
      {
        foreach (string value in parameter.Value)
        {
          parameters.Add(string.Join('=', parameter.Key, value));
        }
      }
      return string.Concat('?', string.Join('&', parameters));
    }
  }

  /// <summary>
  /// Gets or sets the fragment of the URL.
  /// </summary>
  public virtual string? Fragment { get; protected set; }

  /// <summary>
  /// Gets the parameters of the URL. Parameters are replaced from placeholders using the format '{key}'.
  /// </summary>
  public virtual IReadOnlyDictionary<string, string> Parameters => UrlParameters.AsReadOnly();
  /// <summary>
  /// Gets or sets the parameters of the URL.
  /// </summary>
  protected virtual Dictionary<string, string> UrlParameters { get; set; } = [];

  /// <summary>
  /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
  /// </summary>
  public UrlBuilder()
  {
    Scheme = DefaultScheme;
    Host = DefaultHost;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
  /// </summary>
  /// <param name="scheme">The scheme of the URL.</param>
  /// <param name="host">The host of the URL.</param>
  public UrlBuilder(string scheme, string host) : this()
  {
    SetScheme(scheme, inferPort: true);
    SetHost(host);
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
  /// </summary>
  /// <param name="scheme">The scheme of the URL.</param>
  /// <param name="host">The host of the URL.</param>
  /// <param name="port">The port of the URL.</param>
  /// <param name="credentials">The credentials of the URL.</param>
  public UrlBuilder(string scheme, string host, ushort port, ICredentials? credentials = null) : this(scheme, host)
  {
    Port = port;
    SetCredentials(credentials);
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
  /// </summary>
  /// <param name="scheme">The scheme of the URL.</param>
  /// <param name="host">The host of the URL.</param>
  /// <param name="port">The port of the URL.</param>
  /// <param name="path">The path of the URL.</param>
  /// <param name="credentials">The credentials of the URL.</param>
  public UrlBuilder(string scheme, string host, ushort port, string? path, ICredentials? credentials = null)
    : this(scheme, host, port, credentials)
  {
    SetPath(path);
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
  /// </summary>
  /// <param name="scheme">The scheme of the URL.</param>
  /// <param name="host">The host of the URL.</param>
  /// <param name="port">The port of the URL.</param>
  /// <param name="path">The path of the URL.</param>
  /// <param name="queryString">The query string of the URL.</param>
  /// <param name="credentials">The credentials of the URL.</param>
  public UrlBuilder(string scheme, string host, ushort port, string? path, string? queryString, ICredentials? credentials = null)
    : this(scheme, host, port, path, credentials)
  {
    SetQueryString(queryString);
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
  /// </summary>
  /// <param name="scheme">The scheme of the URL.</param>
  /// <param name="host">The host of the URL.</param>
  /// <param name="port">The port of the URL.</param>
  /// <param name="path">The path of the URL.</param>
  /// <param name="queryString">The query string of the URL.</param>
  /// <param name="fragment">The query fragment.</param>
  /// <param name="credentials">The credentials of the URL.</param>
  public UrlBuilder(string scheme, string host, ushort port, string? path, string queryString, string? fragment, ICredentials? credentials = null)
    : this(scheme, host, port, path, queryString, credentials)
  {
    SetFragment(fragment);
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
  /// </summary>
  /// <param name="url">The absolute URL string.</param>
  public UrlBuilder(string url) : this(new Uri(url, UriKind.Absolute))
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
  /// </summary>
  /// <param name="uri">The Uniform Resource Identifier (URI).</param>
  public UrlBuilder(Uri uri) : this()
  {
    SetScheme(uri.Scheme);
    SetCredentials(Net.Credentials.Parse(uri.UserInfo));
    SetHost(uri.Host);
    SetPort((ushort)uri.Port);
    SetPath(uri.AbsolutePath);
    SetQueryString(uri.Query);
    SetFragment(uri.Fragment);
  }

  /// <summary>
  /// Sets the scheme of the URL.
  /// </summary>
  /// <param name="scheme">The URL scheme.</param>
  /// <param name="inferPort">A value indicating whether or not to infer the URL port from the specified scheme.</param>
  /// <returns>The URL builder.</returns>
  /// <exception cref="ArgumentException">The scheme is not supported.</exception>
  public virtual IUrlBuilder SetScheme(string scheme, bool inferPort = false)
  {
    if (!IsSchemeSupported(scheme))
    {
      throw new ArgumentException($"The scheme '{scheme}' is not supported.", nameof(scheme));
    }

    Scheme = scheme.Trim().ToLower();
    if (inferPort)
    {
      Port = InferPort(scheme);
    }
    return this;
  }

  /// <summary>
  /// Sets the user information (username/password) of the URL.
  /// </summary>
  /// <param name="credentials">The user information.</param>
  /// <returns>The URL builder.</returns>
  public virtual IUrlBuilder SetCredentials(ICredentials? credentials)
  {
    Credentials = credentials;
    return this;
  }
  /// <summary>
  /// Sets the host of the URL.
  /// </summary>
  /// <param name="host">The URL host.</param>
  /// <returns>The URL builder.</returns>
  public virtual IUrlBuilder SetHost(string host)
  {
    Host = host.CleanTrim() ?? DefaultHost;
    return this;
  }
  /// <summary>
  /// Sets the port of the URL.
  /// </summary>
  /// <param name="port">The URL port.</param>
  /// <returns>The URL builder.</returns>
  public virtual IUrlBuilder SetPort(ushort port)
  {
    Port = port;
    return this;
  }
  /// <summary>
  /// Sets the authority of the URL.
  /// </summary>
  /// <param name="authority">The authority of the URL.</param>
  /// <returns>The URL builder.</returns>
  /// <exception cref="ArgumentException">The value is not a valid URL authority.</exception>
  public virtual IUrlBuilder SetAuthority(string authority)
  {
    string[] parts = authority.Split('@');
    if (parts.Length > 2)
    {
      throw new ArgumentException($"The value '{authority}' is not a valid URL authority.", nameof(authority));
    }

    int index;
    if (parts.Length == 2)
    {
      SetCredentials(Net.Credentials.Parse(parts.First()));
    }

    string endPoint = parts.Last();
    index = endPoint.IndexOf(':');
    if (index < 0)
    {
      SetHost(endPoint);
    }
    else
    {
      SetHost(endPoint[..index]);
      SetPort(ushort.Parse(endPoint[(index + 1)..]));
    }

    return this;
  }

  /// <summary>
  /// Sets the path segments of the URL.
  /// </summary>
  /// <param name="segments">The path segments.</param>
  /// <returns>The URL builder.</returns>
  public virtual IUrlBuilder SetSegments(IEnumerable<string> segments)
  {
    UrlSegments.Clear();
    foreach (string segment in segments)
    {
      if (!string.IsNullOrWhiteSpace(segment))
      {
        UrlSegments.Add(segment.Trim());
      }
    }
    return this;
  }
  /// <summary>
  /// Sets the path of the URL.
  /// </summary>
  /// <param name="path">The path of the URL.</param>
  /// <returns>The URL builder.</returns>
  public virtual IUrlBuilder SetPath(string? path) => SetSegments(path?.Split('/') ?? []);

  /// <summary>
  /// Appends a query parameter value.
  /// </summary>
  /// <param name="key">The key of the parameter.</param>
  /// <param name="value">The value of the parameter.</param>
  /// <returns>The URL builder.</returns>
  public virtual IUrlBuilder AddQuery(string key, string value) => AddQuery(key, [value]);
  /// <summary>
  /// Appends a list of query parameter values.
  /// </summary>
  /// <param name="key">The key of the parameter.</param>
  /// <param name="values">The values of the parameter.</param>
  /// <returns>The URL builder.</returns>
  public virtual IUrlBuilder AddQuery(string key, IEnumerable<string> values)
  {
    key = key.Trim();
    if (QueryParameters.TryGetValue(key, out List<string>? existingValues))
    {
      foreach (string value in values)
      {
        if (!string.IsNullOrWhiteSpace(value))
        {
          existingValues.Add(value.Trim());
        }
      }
    }
    else
    {
      SetQuery(key, values);
    }
    return this;
  }
  /// <summary>
  /// Sets the value of a query parameter.
  /// </summary>
  /// <param name="key">The key of the parameter.</param>
  /// <param name="value">The value of the parameter.</param>
  /// <returns>The URL builder.</returns>
  public virtual IUrlBuilder SetQuery(string key, string value) => SetQuery(key, [value]);
  /// <summary>
  /// Sets the values of a query parameter.
  /// </summary>
  /// <param name="key">The key of the parameter.</param>
  /// <param name="values">The values of the parameter.</param>
  /// <returns>The URL builder.</returns>
  public virtual IUrlBuilder SetQuery(string key, IEnumerable<string> values)
  {
    key = key.Trim();
    List<string> queryValues = [];
    foreach (string value in values)
    {
      if (!string.IsNullOrWhiteSpace(value))
      {
        queryValues.Add(value.Trim());
      }
    }
    QueryParameters[key] = queryValues;
    return this;
  }
  /// <summary>
  /// Sets the query string of the URL.
  /// </summary>
  /// <param name="queryString">The query string.</param>
  /// <returns>The URL builder.</returns>
  public virtual IUrlBuilder SetQueryString(string? queryString)
  {
    queryString = queryString?.Trim().TrimStart('?').CleanTrim();

    QueryParameters.Clear();
    if (queryString != null)
    {
      string[] parameters = queryString.Split('&');
      foreach (string parameter in parameters)
      {
        int index = parameter.IndexOf('=');
        if (index >= 0)
        {
          AddQuery(parameter[..index], parameter[(index + 1)..]);
        }
      }
    }
    return this;
  }

  /// <summary>
  /// Sets the fragment of the URL.
  /// </summary>
  /// <param name="fragment">The URL fragment.</param>
  /// <returns>The URL builder.</returns>
  public virtual IUrlBuilder SetFragment(string? fragment)
  {
    Fragment = fragment?.Trim().TrimStart('#').CleanTrim();
    return this;
  }

  /// <summary>
  /// Sets an URL parameter.
  /// </summary>
  /// <param name="key">The key of the parameter.</param>
  /// <param name="value">The value of the parameter.</param>
  /// <returns>The URL builder.</returns>
  public virtual IUrlBuilder SetParameter(string key, string? value)
  {
    key = key.Trim();
    if (string.IsNullOrWhiteSpace(value))
    {
      UrlParameters.Remove(key);
    }
    else
    {
      UrlParameters[key] = value.Trim();
    }
    return this;
  }

  /// <summary>
  /// Builds the URL.
  /// </summary>
  /// <param name="kind">The kind of URL to build.</param>
  /// <returns>The built URL.</returns>
  /// <exception cref="ArgumentException">The URL kind is indeterminate.</exception>
  public virtual string Build(UriKind kind = UriKind.Absolute)
  {
    if (kind == UriKind.RelativeOrAbsolute)
    {
      throw new ArgumentException("The URL kind cannot be indeterminate.", nameof(kind));
    }

    StringBuilder url = new();

    if (kind == UriKind.Absolute)
    {
      url.Append(Scheme).Append("://");
      url.Append(Authority);
    }

    if (Path != null)
    {
      url.Append('/').Append(Path);
    }

    if (QueryString != null)
    {
      url.Append(QueryString);
    }

    if (Fragment != null)
    {
      url.Append('#').Append(Fragment);
    }

    string urlString = url.ToString();

    foreach (KeyValuePair<string, string> parameter in UrlParameters)
    {
      string pattern = $"{{{parameter.Key}}}";
      urlString = urlString.Replace(pattern, parameter.Value);
    }

    return urlString;
  }
  /// <summary>
  /// Builds an Uniform Resource Identifier (URI) object.
  /// </summary>
  /// <param name="kind">The kind of URI.</param>
  /// <returns>The built instance.</returns>
  public virtual Uri BuildUri(UriKind kind = UriKind.Absolute) => new(Build(kind), kind);

  /// <summary>
  /// Tries deducing the port of the URL from the specified scheme. The default port is 80.
  /// </summary>
  /// <param name="scheme">The URL scheme.</param>
  /// <returns>The most plausible URL port.</returns>
  protected virtual ushort InferPort(string scheme)
  {
    return scheme.Trim().ToLower() switch
    {
      "https" => 443,
      _ => 80,
    };
  }
}
