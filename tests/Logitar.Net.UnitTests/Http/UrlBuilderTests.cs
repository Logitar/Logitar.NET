namespace Logitar.Net.Http;

[Trait(Traits.Category, Categories.Unit)]
public class UrlBuilderTests
{
  private readonly UrlBuilder _builder = new();

  [Theory(DisplayName = "AddQuery: it should append a query value.")]
  [InlineData(" limit ", "100")]
  public void AddQuery_it_should_append_a_query_value(string key, string value)
  {
    Assert.False(string.IsNullOrWhiteSpace(value));

    string otherValue = Guid.NewGuid().ToString();
    _builder.SetQuery(key, otherValue);
    Assert.Equal([otherValue], _builder.Query[key.Trim()]);

    _builder.AddQuery(key, value);
    Assert.Equal([otherValue, value.Trim()], _builder.Query[key.Trim()]);
  }

  [Theory(DisplayName = "AddQuery: it should append values that are not null or white space.")]
  [InlineData(" id ", "39275fbd-56b0-48b7-bfa5-1417f360b8bd", "  65dd1fba-9bf4-4d6b-90b4-68aa50e4989a  ")]
  public void AddQuery_it_should_append_values_that_are_not_null_or_white_space(string key, params string[] values)
  {
    Assert.All(values, value => Assert.False(string.IsNullOrWhiteSpace(value)));

    string otherValue = Guid.NewGuid().ToString();
    _builder.SetQuery(key, otherValue);
    Assert.Equal([otherValue], _builder.Query[key.Trim()]);

    _builder.AddQuery(key, values);
    Assert.Equal(new[] { otherValue }.Concat(values.Select(value => value.Trim())), _builder.Query[key.Trim()]);
  }

  [Theory(DisplayName = "AddQuery: it should not append a null or white space value.")]
  [InlineData(" limit ", null)]
  [InlineData(" limit ", "")]
  [InlineData(" limit ", "  ")]
  public void AddQuery_it_should_not_append_null_or_white_space_values(string key, string? value)
  {
    Assert.True(string.IsNullOrWhiteSpace(value));

    string otherValue = Guid.NewGuid().ToString();
    _builder.SetQuery(key, otherValue);
    Assert.Equal([otherValue], _builder.Query[key.Trim()]);

    _builder.AddQuery(key, value!);
    Assert.Equal([otherValue], _builder.Query[key.Trim()]);

    _builder.AddQuery(key, [null!, string.Empty, "  "]);
    Assert.Equal([otherValue], _builder.Query[key.Trim()]);
  }

  [Theory(DisplayName = "Authority: it should return the correct authority.")]
  [InlineData("localhost", 8080)]
  [InlineData(" www.test.com ", 443)]
  [InlineData("www.test.com", 443, "admin")]
  [InlineData("www.test.com", 443, "admin", "P@s$W0rD")]
  public void Authority_it_should_return_the_correct_authority(string host, ushort port, string? identifier = null, string? secret = null)
  {
    _builder.SetHost(host);
    _builder.SetPort(port);

    Credentials? credentials = null;
    if (identifier != null)
    {
      credentials = new(identifier, secret ?? string.Empty);
      _builder.SetCredentials(credentials);
    }

    string authority = credentials == null ? $"{host.Trim()}:{port}" : $"{identifier}:{secret}@{host.Trim()}:{port}";
    Assert.Equal(authority, _builder.Authority);
  }

  [Fact(DisplayName = "Build: it should build the correct absolute URL.")]
  public void Build_it_should_build_the_correct_absolute_Url()
  {
    string id = Guid.NewGuid().ToString();

    _builder.SetScheme("https", inferPort: true);
    _builder.SetCredentials(new Credentials("admin", "Test123!"));
    _builder.SetHost("www.test.com");
    _builder.SetPath("/people//{id}/");
    _builder.AddQuery("ref", "1234567890");
    _builder.AddQuery("q", "francis");
    _builder.AddQuery("q", "pion");
    _builder.SetFragment(" #  profile ");
    _builder.SetParameter(" id ", id);

    string url = _builder.Build(UriKind.Absolute);
    Assert.Equal($"https://admin:Test123!@www.test.com:443/people/{id}?ref=1234567890&q=francis&q=pion#profile", url);
  }

  [Fact(DisplayName = "Build: it should build the correct relative URL.")]
  public void Build_it_should_build_the_correct_relative_Url()
  {
    string id = Guid.NewGuid().ToString();

    _builder.SetScheme("https", inferPort: true);
    _builder.SetCredentials(new Credentials("admin", "Test123!"));
    _builder.SetHost("www.test.com");
    _builder.SetPath("/people//{id}/");
    _builder.AddQuery("ref", "1234567890");
    _builder.AddQuery("q", "francis");
    _builder.AddQuery("q", "pion");
    _builder.SetFragment(" #  profile ");
    _builder.SetParameter(" id ", id);

    string url = _builder.Build(UriKind.Relative);
    Assert.Equal($"/people/{id}?ref=1234567890&q=francis&q=pion#profile", url);
  }

  [Fact(DisplayName = "Build: it should throw ArgumentException when the kind is indeterminate.")]
  public void Build_it_should_throw_ArgumentException_when_the_kind_is_indeterminate()
  {
    var exception = Assert.Throws<ArgumentException>(() => _builder.Build(UriKind.RelativeOrAbsolute));
    Assert.StartsWith("The URL kind cannot be indeterminate.", exception.Message);
    Assert.Equal("kind", exception.ParamName);
  }

  [Fact(DisplayName = "BuildUri: it should build the correct absolute URI.")]
  public void BuildUri_it_should_build_the_correct_absolute_Uri()
  {
    string id = Guid.NewGuid().ToString();

    _builder.SetScheme("https", inferPort: true);
    _builder.SetCredentials(new Credentials("admin", "Test123!"));
    _builder.SetHost("www.test.com");
    _builder.SetPath("/people//{id}/");
    _builder.AddQuery("ref", "1234567890");
    _builder.AddQuery("q", "francis");
    _builder.AddQuery("q", "pion");
    _builder.SetFragment(" #  profile ");
    _builder.SetParameter(" id ", id);

    Uri uri = _builder.BuildUri(UriKind.Absolute);
    Assert.Equal($"https://admin:Test123!@www.test.com/people/{id}?ref=1234567890&q=francis&q=pion#profile", uri.ToString());
  }

  [Fact(DisplayName = "BuildUri: it should build the correct relative URI.")]
  public void BuildUri_it_should_build_the_correct_relative_Uri()
  {
    string id = Guid.NewGuid().ToString();

    _builder.SetScheme("https", inferPort: true);
    _builder.SetCredentials(new Credentials("admin", "Test123!"));
    _builder.SetHost("www.test.com");
    _builder.SetPath("/people//{id}/");
    _builder.AddQuery("ref", "1234567890");
    _builder.AddQuery("q", "francis");
    _builder.AddQuery("q", "pion");
    _builder.SetFragment(" #  profile ");
    _builder.SetParameter(" id ", id);

    Uri uri = _builder.BuildUri(UriKind.Relative);
    Assert.Equal($"/people/{id}?ref=1234567890&q=francis&q=pion#profile", uri.ToString());
  }

  [Fact(DisplayName = "BuildUri: it should throw ArgumentException when the kind is indeterminate.")]
  public void BuildUri_it_should_throw_ArgumentException_when_the_kind_is_indeterminate()
  {
    var exception = Assert.Throws<ArgumentException>(() => _builder.BuildUri(UriKind.RelativeOrAbsolute));
    Assert.StartsWith("The URL kind cannot be indeterminate.", exception.Message);
    Assert.Equal("kind", exception.ParamName);
  }

  [Fact(DisplayName = "ctor: it should construct the correct builder from an absolute URI.")]
  public void ctor_it_should_construct_the_correct_builder_from_an_absolute_Uri()
  {
    Uri uri = new("http://admin:Test123!@localhost:8080/people/fpion?ref=1234567890#profile", UriKind.Absolute);
    UrlBuilder builder = new(uri);
    Assert.Equal("http", builder.Scheme);
    Assert.NotNull(builder.Credentials);
    Assert.Equal("admin", builder.Credentials.Identifier);
    Assert.Equal("Test123!", builder.Credentials.Secret);
    Assert.Equal("localhost", builder.Host);
    Assert.Equal(8080, builder.Port);
    Assert.Equal("people/fpion", builder.Path);
    Assert.Equal("?ref=1234567890", builder.QueryString);
    Assert.Equal("#profile", builder.Fragment);
  }

  [Fact(DisplayName = "ctor: it should construct the correct builder from an absolute URL.")]
  public void ctor_it_should_construct_the_correct_builder_from_an_absolute_Url()
  {
    string url = "http://admin:Test123!@localhost:8080/people/fpion?ref=1234567890#profile";
    UrlBuilder builder = new(url);
    Assert.Equal("http", builder.Scheme);
    Assert.NotNull(builder.Credentials);
    Assert.Equal("admin", builder.Credentials.Identifier);
    Assert.Equal("Test123!", builder.Credentials.Secret);
    Assert.Equal("localhost", builder.Host);
    Assert.Equal(8080, builder.Port);
    Assert.Equal("people/fpion", builder.Path);
    Assert.Equal("?ref=1234567890", builder.QueryString);
    Assert.Equal("#profile", builder.Fragment);
  }

  [Fact(DisplayName = "ctor: it should construct the correct builder.")]
  public void ctor_it_should_construct_the_correct_builder()
  {
    string scheme = "https";
    string host = "localhost";
    ushort port = 8081;
    Credentials credentials = new("admin", "P@s$W0rD");
    string path = "people/fpion";
    string queryString = $"?ref={Guid.NewGuid()}";
    string fragment = "#profile";

    UrlBuilder builder;

    builder = new();
    Assert.Equal(UrlBuilder.DefaultScheme, builder.Scheme);
    Assert.Null(builder.Credentials);
    Assert.Equal(UrlBuilder.DefaultHost, builder.Host);
    Assert.Equal(80, builder.Port);
    Assert.Null(builder.Path);
    Assert.Null(builder.QueryString);
    Assert.Null(builder.Fragment);
    Assert.Empty(builder.Parameters);

    builder = new(scheme, host);
    Assert.Equal(scheme.Trim().ToLower(), builder.Scheme);
    Assert.Null(builder.Credentials);
    Assert.Equal(host.Trim(), builder.Host);
    Assert.Equal(443, builder.Port);
    Assert.Null(builder.Path);
    Assert.Null(builder.QueryString);
    Assert.Null(builder.Fragment);
    Assert.Empty(builder.Parameters);

    builder = new(scheme, host, port);
    Assert.Equal(scheme.Trim().ToLower(), builder.Scheme);
    Assert.Null(builder.Credentials);
    Assert.Equal(host.Trim(), builder.Host);
    Assert.Equal(port, builder.Port);
    Assert.Null(builder.Path);
    Assert.Null(builder.QueryString);
    Assert.Null(builder.Fragment);
    Assert.Empty(builder.Parameters);

    builder = new(scheme, host, port, credentials);
    Assert.Equal(scheme.Trim().ToLower(), builder.Scheme);
    Assert.Equal(credentials, builder.Credentials);
    Assert.Equal(host.Trim(), builder.Host);
    Assert.Equal(port, builder.Port);
    Assert.Null(builder.Path);
    Assert.Null(builder.QueryString);
    Assert.Null(builder.Fragment);
    Assert.Empty(builder.Parameters);

    builder = new(scheme, host, port, path, credentials);
    Assert.Equal(scheme.Trim().ToLower(), builder.Scheme);
    Assert.Equal(credentials, builder.Credentials);
    Assert.Equal(host.Trim(), builder.Host);
    Assert.Equal(port, builder.Port);
    Assert.Equal(path, builder.Path);
    Assert.Null(builder.QueryString);
    Assert.Null(builder.Fragment);
    Assert.Empty(builder.Parameters);

    builder = new(scheme, host, port, path, queryString, credentials);
    Assert.Equal(scheme.Trim().ToLower(), builder.Scheme);
    Assert.Equal(credentials, builder.Credentials);
    Assert.Equal(host.Trim(), builder.Host);
    Assert.Equal(port, builder.Port);
    Assert.Equal(path, builder.Path);
    Assert.Equal(queryString, builder.QueryString);
    Assert.Null(builder.Fragment);
    Assert.Empty(builder.Parameters);

    builder = new(scheme, host, port, path, queryString, fragment, credentials);
    Assert.Equal(scheme.Trim().ToLower(), builder.Scheme);
    Assert.Equal(credentials, builder.Credentials);
    Assert.Equal(host.Trim(), builder.Host);
    Assert.Equal(port, builder.Port);
    Assert.Equal(path, builder.Path);
    Assert.Equal(queryString, builder.QueryString);
    Assert.Equal(fragment, builder.Fragment);
    Assert.Empty(builder.Parameters);
  }

  [Fact(DisplayName = "InferPort: it should infer the correct port.")]
  public void InferPort_it_should_infer_the_correct_port()
  {
    _builder.SetScheme("http", inferPort: true);
    Assert.Equal(80, _builder.Port);

    _builder.SetScheme("https", inferPort: true);
    Assert.Equal(443, _builder.Port);
  }

  [Theory(DisplayName = "IsSchemeSupported: it should return false when the scheme is not supported.")]
  [InlineData("ftp")]
  [InlineData("mailto")]
  public void IsSchemeSupported_it_should_return_false_when_the_scheme_is_not_supported(string scheme)
  {
    Assert.False(UrlBuilder.IsSchemeSupported(scheme));
  }

  [Fact(DisplayName = "IsSchemeSupported: it should return true when the scheme is supported.")]
  public void IsSchemeSupported_it_should_return_true_when_the_scheme_is_supported()
  {
    foreach (string scheme in UrlBuilder.SupportedSchemes)
    {
      Assert.True(UrlBuilder.IsSchemeSupported($"  {scheme.ToUpper()}  "));
    }
  }

  [Fact(DisplayName = "QueryString: it should return the correct query string.")]
  public void QueryString_it_should_return_the_correct_query_string()
  {
    _builder.AddQuery("ids", "09782db2-1348-4135-943a-fcbdab80799f");
    _builder.AddQuery("sort", "desc.UpdatedOn");
    _builder.AddQuery("skip", string.Empty);
    _builder.AddQuery(" ids ", " ee0545c1-de76-40f2-84ab-ad2574c71e0b ");
    _builder.AddQuery("limit", "2");

    string queryString = "?ids=09782db2-1348-4135-943a-fcbdab80799f&ids=ee0545c1-de76-40f2-84ab-ad2574c71e0b&sort=desc.UpdatedOn&limit=2";
    Assert.Equal(queryString, _builder.QueryString);
  }

  [Theory(DisplayName = "SetAuthority: it should set the correct authority.")]
  [InlineData("localhost", 8080)]
  [InlineData(" www.test.com ", 443)]
  [InlineData("www.test.com", 443, "admin")]
  [InlineData("www.test.com", 443, "admin", "Test123!")]
  public void SetAuthority_it_should_set_the_correct_authority(string host, ushort port, string? identifier = null, string? secret = null)
  {
    Credentials? credentials = identifier == null ? null : new(identifier, secret ?? string.Empty);
    string authority = credentials == null ? $"{host.Trim()}:{port}" : $"{identifier}:{secret}@{host.Trim()}:{port}";
    _builder.SetAuthority(authority);

    Assert.Equal(host.Trim(), _builder.Host);
    Assert.Equal(port, _builder.Port);

    if (credentials == null)
    {
      Assert.Null(_builder.Credentials);
    }
    else
    {
      Assert.Equal(credentials, _builder.Credentials);
    }
  }

  [Theory(DisplayName = "SetAuthority: it should set the correct authority without port.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  [InlineData("localhost")]
  [InlineData("  localhost  ")]
  public void SetAuthority_it_should_set_the_correct_authority_without_port(string? host)
  {
    ushort port = 12345;
    _builder.SetPort(port);

    _builder.SetAuthority(host ?? string.Empty);
    Assert.Null(_builder.Credentials);
    Assert.Equal(string.IsNullOrWhiteSpace(host) ? UrlBuilder.DefaultHost : host.Trim(), _builder.Host);
    Assert.Equal(port, _builder.Port);
  }

  [Fact(DisplayName = "SetAuthority: it should throw ArgumentException when it is not a valid authority.")]
  public void SetAuthority_it_should_throw_ArgumentException_when_it_is_not_a_valid_authority()
  {
    string authority = "admin:P@s$W0rD@localhost:8080";
    var exception = Assert.Throws<ArgumentException>(() => _builder.SetAuthority(authority));
    Assert.StartsWith($"The value '{authority}' is not a valid URL authority.", exception.Message);
    Assert.Equal("authority", exception.ParamName);
  }

  [Theory(DisplayName = "SetCredentials: it should set the correct credentials.")]
  [InlineData("admin", "P@s$W0rD")]
  public void SetCredentials_it_should_set_the_correct_credentials(string identifier, string secret)
  {
    Assert.Null(_builder.Credentials);

    Credentials credentials = new(identifier, secret);
    _builder.SetCredentials(credentials);
    Assert.Equal(credentials, _builder.Credentials);

    _builder.SetCredentials(credentials: null);
    Assert.Null(_builder.Credentials);
  }

  [Theory(DisplayName = "SetFragment: it should set the correct fragment.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  [InlineData(" # ")]
  [InlineData("#index")]
  [InlineData(" #profile ")]
  public void SetFragment_it_should_set_the_correct_fragment(string? fragment)
  {
    _builder.SetFragment(fragment);
    if (string.IsNullOrWhiteSpace(fragment) || fragment.Trim() == "#")
    {
      Assert.Null(_builder.Fragment);
    }
    else
    {
      Assert.Equal(fragment.Trim(), _builder.Fragment);
    }
  }

  [Theory(DisplayName = "SetHost: it should set the correct host.")]
  [InlineData("")]
  [InlineData("   ")]
  [InlineData("  localhost  ")]
  [InlineData("www.test.com")]
  public void SetHost_it_should_set_the_correct_host(string host)
  {
    _builder.SetHost(host);
    if (string.IsNullOrWhiteSpace(host))
    {
      Assert.Equal(UrlBuilder.DefaultHost, _builder.Host);
    }
    else
    {
      Assert.Equal(host.Trim(), _builder.Host);
    }
  }

  [Theory(DisplayName = "SetParameter: it should set the correct parameter.")]
  [InlineData("id", null)]
  [InlineData("id", "")]
  [InlineData("id", "  ")]
  [InlineData("id", "e3720d60-19ba-430b-81cb-def77c91c19a")]
  [InlineData(" id ", " d910c659-0c05-466c-8347-270c6570e179 ")]
  public void SetParameter_it_should_set_the_correct_parameter(string key, string? value)
  {
    if (string.IsNullOrWhiteSpace(value))
    {
      _builder.SetParameter(key, Guid.NewGuid().ToString());
      Assert.True(_builder.Parameters.ContainsKey(key.Trim()));

      _builder.SetParameter(key, value);
      Assert.False(_builder.Parameters.ContainsKey(key.Trim()));
    }
    else
    {
      _builder.SetParameter(key, value);
      Assert.Equal(value.Trim(), _builder.Parameters[key.Trim()]);
    }
  }

  [Theory(DisplayName = "SetParameter: it should throw ArgumentException when the key is null or white space.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void SetParameter_it_should_throw_ArgumentException_when_the_key_is_null_or_white_space(string? key)
  {
    Assert.True(string.IsNullOrWhiteSpace(key));

    var exception = Assert.Throws<ArgumentException>(() => _builder.SetParameter(key!, value: null));
    Assert.StartsWith("The parameter key is required.", exception.Message);
    Assert.Equal("key", exception.ParamName);
  }

  [Theory(DisplayName = "SetPath: it should set the correct path and segments.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  [InlineData(" people/fpion ")]
  [InlineData(" people/ /fpion ")]
  [InlineData(" /people//fpion/profile/ ")]
  public void SetPath_it_should_set_the_correct_path_and_segments(string? path)
  {
    List<string> cleanSegments = [];
    if (path != null)
    {
      foreach (string value in path.Split('/'))
      {
        if (!string.IsNullOrWhiteSpace(value))
        {
          cleanSegments.Add(value.Trim());
        }
      }
    }
    string cleanPath = string.Join('/', cleanSegments);

    _builder.SetPath(path);
    Assert.Equal(cleanSegments, _builder.Segments);
    if (cleanSegments.Count == 0)
    {
      Assert.Null(_builder.Path);
    }
    else
    {
      Assert.Equal(cleanPath, _builder.Path);
    }
  }

  [Theory(DisplayName = "SetPort: it should set the correct port.")]
  [InlineData(12345)]
  public void SetPort_it_should_set_the_correct_port(ushort port)
  {
    _builder.SetPort(port);
    Assert.Equal(port, _builder.Port);
  }

  [Theory(DisplayName = "SetQuery: it should not set a query parameter when the key is null or white space.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void SetQuery_it_should_not_set_a_query_parameter_when_the_key_is_null_or_white_space(string? key)
  {
    Assert.True(string.IsNullOrWhiteSpace(key));

    string value = Guid.NewGuid().ToString();
    _builder.SetQuery("id", value);
    Assert.Equal([value], _builder.Query["id"]);

    _builder.SetQuery(key!, value);
    Assert.Equal([value], _builder.Query["id"]);

    _builder.SetQuery(key!, ["1", "2", "3"]);
    Assert.Equal([value], _builder.Query["id"]);
  }

  [Theory(DisplayName = "SetQuery: it should remove a query parameter when all values are null or white space.")]
  [InlineData("id")]
  public void SetQuery_it_should_remove_a_query_parameter_when_all_values_are_null_or_white_space(string key)
  {
    Assert.False(string.IsNullOrWhiteSpace(key));

    string otherValue = Guid.NewGuid().ToString();
    _builder.SetQuery(key, otherValue);
    Assert.Equal([otherValue], _builder.Query[key.Trim()]);

    _builder.SetQuery(key, [null!, string.Empty, "   "]);
    Assert.False(_builder.Query.ContainsKey(key.Trim()));
  }

  [Theory(DisplayName = "SetQuery: it should remove a query parameter when the value is null or white space.")]
  [InlineData("id", null)]
  [InlineData("id", "")]
  [InlineData("id", "  ")]
  public void SetQuery_it_should_remove_a_query_parameter_when_the_value_is_null_or_white_space(string key, string? value)
  {
    Assert.False(string.IsNullOrWhiteSpace(key));

    string otherValue = Guid.NewGuid().ToString();
    _builder.SetQuery(key, otherValue);
    Assert.Equal([otherValue], _builder.Query[key.Trim()]);

    _builder.SetQuery(key, value!);
    Assert.False(_builder.Query.ContainsKey(key.Trim()));
  }

  [Theory(DisplayName = "SetQuery: it should set the correct query parameter value.")]
  [InlineData("id", "f52e721d-8343-4606-adec-f5a375993bd6")]
  [InlineData(" id ", " 22b5072b-e5c9-4e96-85b9-99c4001d667c ")]
  public void SetQuery_it_should_set_the_correct_query_parameter_value(string key, string value)
  {
    string otherValue = new(value.Reverse().ToArray());
    _builder.SetQuery(key, otherValue);
    Assert.Equal([otherValue.Trim()], _builder.Query[key.Trim()]);

    _builder.SetQuery(key, value);
    Assert.Equal([value.Trim()], _builder.Query[key.Trim()]);
  }

  [Theory(DisplayName = "SetQuery: it should set the correct query parameter values.")]
  [InlineData("id", "6d99b570-c1b0-44de-9a7c-b70b6715d0bf")]
  [InlineData(" id ", "", "   ", " f32a7358-453c-4180-b056-7f94f36ace5b ")]
  public void SetQuery_it_should_set_the_correct_query_parameter_values(string key, params string[] values)
  {
    IEnumerable<string> cleanValues = values.Where(value => !string.IsNullOrWhiteSpace(value)).Select(value => value.Trim());

    string otherValue = Guid.NewGuid().ToString();
    _builder.SetQuery(key, [otherValue]);
    Assert.Equal([otherValue], _builder.Query[key.Trim()]);

    _builder.SetQuery(key, values);
    Assert.Equal(cleanValues, _builder.Query[key.Trim()]);
  }

  [Theory(DisplayName = "SetQueryString: it should reset the query string when it is null or white space.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("?")]
  [InlineData("  ")]
  [InlineData(" ? ")]
  public void SetQueryString_it_should_reset_the_query_string_when_it_is_null_or_white_space(string? queryString)
  {
    _builder.SetQuery("id", Guid.NewGuid().ToString());
    Assert.NotEmpty(_builder.Query);

    _builder.SetQueryString(queryString);
    Assert.Empty(_builder.Query);
  }

  [Fact(DisplayName = "SetQueryString: it should set the correct query string.")]
  public void SetQueryString_it_should_set_the_correct_query_string()
  {
    string queryString = "  ?ids=09782db2-1348-4135-943a-fcbdab80799f&sort=desc.UpdatedOn&skip=& ids = ee0545c1-de76-40f2-84ab-ad2574c71e0b &limit=2";
    _builder.SetQueryString(queryString);

    Assert.Equal(3, _builder.Query.Count);
    Assert.Contains(_builder.Query, q => q.Key == "ids" && q.Value.SequenceEqual(["09782db2-1348-4135-943a-fcbdab80799f", "ee0545c1-de76-40f2-84ab-ad2574c71e0b"]));
    Assert.Contains(_builder.Query, q => q.Key == "sort" && q.Value.SequenceEqual(["desc.UpdatedOn"]));
    Assert.Contains(_builder.Query, q => q.Key == "limit" && q.Value.SequenceEqual(["2"]));
  }

  [Theory(DisplayName = "SetScheme: it should set the correct scheme and port.")]
  [InlineData("http", false)]
  [InlineData("https", true)]
  public void SetScheme_it_should_set_the_correct_scheme_and_port(string scheme, bool inferPort)
  {
    UrlBuilder builder = new("http", "localhost", 8080);
    builder.SetScheme(scheme, inferPort);
    Assert.Equal(scheme, builder.Scheme);
    if (inferPort)
    {
      Assert.Equal(scheme.Trim().ToLower() == "http" ? 80 : 443, builder.Port);
    }
    else
    {
      Assert.Equal(8080, builder.Port);
    }
  }

  [Theory(DisplayName = "SetScheme: it should throw ArgumentException when the scheme is not supported.")]
  [InlineData("ftp")]
  [InlineData("mailto")]
  public void SetScheme_it_should_throw_ArgumentException_when_the_scheme_is_not_supported(string scheme)
  {
    var exception = Assert.Throws<ArgumentException>(() => _builder.SetScheme(scheme));
    Assert.StartsWith($"The scheme '{scheme}' is not supported.", exception.Message);
    Assert.Equal("scheme", exception.ParamName);
  }

  [Theory(DisplayName = "SetSegments: it should set the correct path and segments.")]
  [InlineData("")]
  [InlineData("  ")]
  [InlineData(" people", "fpion ")]
  [InlineData(" people", " ", "fpion ")]
  [InlineData(" ", "people", "", "", "fpion", "profile", " ")]
  public void SetSegments_it_should_set_the_correct_path_and_segments(params string[] segments)
  {
    List<string> cleanSegments = [];
    foreach (string value in segments)
    {
      if (!string.IsNullOrWhiteSpace(value))
      {
        cleanSegments.Add(value.Trim());
      }
    }
    string cleanPath = string.Join('/', cleanSegments);

    _builder.SetSegments(segments);
    Assert.Equal(cleanSegments, _builder.Segments);
    if (cleanSegments.Count == 0)
    {
      Assert.Null(_builder.Path);
    }
    else
    {
      Assert.Equal(cleanPath, _builder.Path);
    }
  }
}
