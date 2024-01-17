namespace Logitar.Net.Http;

public class UrlBuilder : IUrlBuilder
{
  /// <summary>
  /// Gets the Uniform Resource Locator (URL) parameters.
  /// </summary>
  public virtual IReadOnlyDictionary<string, string> Parameters => UrlParameters.AsReadOnly();
  /// <summary>
  /// Gets the Uniform Resource Locator (URL) query parameters.
  /// </summary>
  public virtual IReadOnlyDictionary<string, IEnumerable<string>> Query => UrlQuery.AsReadOnly();
  /// <summary>
  /// Gets the query string.
  /// </summary>
  public virtual string QueryString
  {
    get
    {
      List<string> parameters = [];

      foreach (KeyValuePair<string, IEnumerable<string>> query in UrlQuery)
      {
        foreach (string value in query.Value)
        {
          if (!string.IsNullOrEmpty(value))
          {
            parameters.Add($"{query.Key}={value}");
          }
        }
      }

      return parameters.Count == 0 ? string.Empty : string.Concat('?', string.Join('&', parameters));
    }
  }

  /// <summary>
  /// Gets or sets the Uniform Resource Locator (URL) parameters.
  /// </summary>
  protected Dictionary<string, string> UrlParameters { get; set; } = [];
  /// <summary>
  /// Gets or sets the Uniform Resource Locator (URL) query parameters.
  /// </summary>
  protected Dictionary<string, IEnumerable<string>> UrlQuery { get; set; } = [];

  /// <summary>
  /// Sets an Uniform Resource Locator (URL) parameter.
  /// </summary>
  /// <param name="key">The key of the parameter.</param>
  /// <param name="value">The value of the parameter.</param>
  /// <returns>The builder instance.</returns>
  public virtual IUrlBuilder SetParameter(string key, string value)
  {
    UrlParameters[key] = value;
    return this;
  }

  /// <summary>
  /// Adds an Uniform Resource Locator (URL) query parameter.
  /// </summary>
  /// <param name="key">The key of the parameter.</param>
  /// <param name="value">The value of the parameter.</param>
  /// <returns>The builder instance.</returns>
  public virtual IUrlBuilder AddQuery(string key, string value) => AddQuery(key, [value]);
  /// <summary>
  /// Adds an Uniform Resource Locator (URL) query parameter.
  /// </summary>
  /// <param name="key">The key of the parameter.</param>
  /// <param name="values">The values of the parameter.</param>
  /// <returns>The builder instance.</returns>
  public virtual IUrlBuilder AddQuery(string key, IEnumerable<string> values)
  {
    if (UrlQuery.TryGetValue(key, out IEnumerable<string>? existingValues))
    {
      UrlQuery[key] = existingValues.Concat(values);
    }
    else
    {
      UrlQuery[key] = values;
    }
    return this;
  }
  /// <summary>
  /// Sets an Uniform Resource Locator (URL) query parameter.
  /// </summary>
  /// <param name="key">The key of the parameter.</param>
  /// <param name="value">The value of the parameter.</param>
  /// <returns>The builder instance.</returns>
  public virtual IUrlBuilder SetQuery(string key, string value) => SetQuery(key, [value]);
  /// <summary>
  /// Sets an Uniform Resource Locator (URL) query parameter.
  /// </summary>
  /// <param name="key">The key of the parameter.</param>
  /// <param name="values">The values of the parameter.</param>
  /// <returns>The builder instance.</returns>
  public virtual IUrlBuilder SetQuery(string key, IEnumerable<string> values)
  {
    UrlQuery[key] = values;
    return this;
  }

  /// <summary>
  /// Builds the request Uniform Resource Identifier (URI).
  /// </summary>
  /// <returns>The built instance.</returns>
  public virtual Uri BuildUri()
  {
    string url = string.Empty; //string url = Url;

    foreach (KeyValuePair<string, string> parameter in UrlParameters)
    {
      url = url.Replace($"{{{parameter.Key}}}", parameter.Value);
    }

    return new UriBuilder(url) { Query = QueryString }.Uri;
  }
}
