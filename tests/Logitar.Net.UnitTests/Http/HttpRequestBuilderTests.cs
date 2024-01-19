﻿using Bogus;

namespace Logitar.Net.Http;

[Trait(Traits.Category, Categories.Unit)]
public class HttpRequestBuilderTests
{
  private readonly HttpRequestBuilder _builder = new();
  private readonly Faker _faker = new();
  private readonly Person _person;

  public HttpRequestBuilderTests()
  {
    _person = new(_faker.Person.FirstName, _faker.Person.LastName, _faker.Person.DateOfBirth);
  }

  [Fact(DisplayName = "BuildMessage: it should build the correct HttpRequestMessage.")]
  public void BuildMessage_it_should_build_the_correct_HttpRequestMessage()
  {
    JsonContent content = JsonContent.Create(_person);

    string url = "/people/123?version=2";
    HttpHeader clientId = new("ClientId", Guid.NewGuid().ToString());
    HttpRequestMessage message = HttpRequestBuilder.Put(url)
      .SetContent(content)
      .WithBasicAuthorization("admin", "P@s$W0rD")
      .SetHeader(clientId)
      .BuildMessage();

    Assert.Equal(HttpMethod.Put, message.Method);
    Assert.Equal(url, message.RequestUri?.ToString());
    Assert.Same(content, message.Content);

    AuthenticationHeaderValue? authorization = message.Headers.Authorization;
    Assert.NotNull(authorization);
    string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes("admin:P@s$W0rD"));
    Assert.Equal(AuthenticationSchemes.Basic, authorization.Scheme);
    Assert.Equal(credentials, authorization.Parameter);

    Assert.Contains(message.Headers, h => h.Key == clientId.Name && h.Value.SequenceEqual(clientId.Values));
  }

  [Fact(DisplayName = "BuildMessage: it should build the correct HttpRequestMessage from the specified parameters.")]
  public void BuildMessage_it_should_build_the_correct_HttpRequestMessage_from_the_specified_parameters()
  {
    HttpRequestParameters parameters = new()
    {
      Method = HttpMethod.Put,
      Uri = new("/people/123", UriKind.Relative),
      Content = JsonContent.Create(_person),
      Authorization = HttpAuthorization.Bearer(Guid.NewGuid().ToString())
    };
    parameters.Headers.Add(new HttpHeader("ClientId", Guid.NewGuid().ToString()));

    HttpRequestMessage request = HttpRequestBuilder.BuildMessage(parameters);
    Assert.Equal(parameters.Method, request.Method);
    Assert.Equal(parameters.Uri, request.RequestUri);
    Assert.Same(parameters.Content, request.Content);

    foreach (HttpHeader header in parameters.Headers)
    {
      Assert.Contains(request.Headers, h => h.Key == header.Name && h.Value.SequenceEqual(header.Values));
    }

    Assert.NotNull(request.Headers.Authorization);
    Assert.Equal(parameters.Authorization.Scheme, request.Headers.Authorization.Scheme);
    Assert.Equal(parameters.Authorization.Credentials, request.Headers.Authorization.Parameter);
  }

  [Theory(DisplayName = "ctor: it should construct the correct builder.")]
  [InlineData("GET", "/people")]
  public void ctor_it_should_construct_the_correct_builder(string methodString, string urlString)
  {
    Assert.Equal(HttpMethod.Get.Method, _builder.Method);
    Assert.Equal(string.Empty, _builder.Url);

    HttpRequestBuilder builder = new(methodString, urlString);
    Assert.Equal(methodString, builder.Method);
    Assert.Equal(urlString, builder.Url);

    HttpMethod method = new(methodString);
    Uri uri = new(urlString, UriKind.RelativeOrAbsolute);
    builder = new(method, uri);
    Assert.Equal(methodString, builder.Method);
    Assert.Equal(urlString, builder.Url);
  }

  [Theory(DisplayName = "Delete: it should construct the correct builder.")]
  [InlineData("/people/123")]
  public void Delete_it_should_construct_the_correct_builder(string urlString)
  {
    HttpRequestBuilder builder = HttpRequestBuilder.Delete(urlString);
    Assert.Equal(HttpMethod.Delete.Method, builder.Method);
    Assert.Equal(urlString, builder.Url);

    Uri uri = new(urlString, UriKind.RelativeOrAbsolute);
    builder = HttpRequestBuilder.Delete(uri);
    Assert.Equal(HttpMethod.Delete.Method, builder.Method);
    Assert.Equal(urlString, builder.Url);
  }

  [Theory(DisplayName = "Get: it should construct the correct builder.")]
  [InlineData("/people/123")]
  public void Get_it_should_construct_the_correct_builder(string urlString)
  {
    HttpRequestBuilder builder = HttpRequestBuilder.Get(urlString);
    Assert.Equal(HttpMethod.Get.Method, builder.Method);
    Assert.Equal(urlString, builder.Url);

    Uri uri = new(urlString, UriKind.RelativeOrAbsolute);
    builder = HttpRequestBuilder.Get(uri);
    Assert.Equal(HttpMethod.Get.Method, builder.Method);
    Assert.Equal(urlString, builder.Url);
  }

  [Theory(DisplayName = "Patch: it should construct the correct builder.")]
  [InlineData("/people/123")]
  public void Patch_it_should_construct_the_correct_builder(string urlString)
  {
    HttpRequestBuilder builder = HttpRequestBuilder.Patch(urlString);
    Assert.Equal(HttpMethod.Patch.Method, builder.Method);
    Assert.Equal(urlString, builder.Url);

    Uri uri = new(urlString, UriKind.RelativeOrAbsolute);
    builder = HttpRequestBuilder.Patch(uri);
    Assert.Equal(HttpMethod.Patch.Method, builder.Method);
    Assert.Equal(urlString, builder.Url);
  }

  [Theory(DisplayName = "Post: it should construct the correct builder.")]
  [InlineData("/people/123")]
  public void Post_it_should_construct_the_correct_builder(string urlString)
  {
    HttpRequestBuilder builder = HttpRequestBuilder.Post(urlString);
    Assert.Equal(HttpMethod.Post.Method, builder.Method);
    Assert.Equal(urlString, builder.Url);

    Uri uri = new(urlString, UriKind.RelativeOrAbsolute);
    builder = HttpRequestBuilder.Post(uri);
    Assert.Equal(HttpMethod.Post.Method, builder.Method);
    Assert.Equal(urlString, builder.Url);
  }

  [Theory(DisplayName = "Put: it should construct the correct builder.")]
  [InlineData("/people/123")]
  public void Put_it_should_construct_the_correct_builder(string urlString)
  {
    HttpRequestBuilder builder = HttpRequestBuilder.Put(urlString);
    Assert.Equal(HttpMethod.Put.Method, builder.Method);
    Assert.Equal(urlString, builder.Url);

    Uri uri = new(urlString, UriKind.RelativeOrAbsolute);
    builder = HttpRequestBuilder.Put(uri);
    Assert.Equal(HttpMethod.Put.Method, builder.Method);
    Assert.Equal(urlString, builder.Url);
  }

  [Theory(DisplayName = "SetContent: it should set the correct content.")]
  [InlineData("Hello World!")]
  public void SetContent_it_should_set_the_correct_content(string message)
  {
    Assert.Null(_builder.Content);

    StringContent content = new(message, Encoding.UTF8, MediaTypeNames.Text.Plain);
    _builder.SetContent(content);
    Assert.Same(content, _builder.Content);

    _builder.SetContent(content: null);
    Assert.Null(_builder.Content);
  }

  [Theory(DisplayName = "SetHeader: it should set the correct header.")]
  [InlineData("Accept", "application/json", "text/html")]
  public void SetHeader_it_should_set_the_correct_header(string name, params string[] values)
  {
    _builder.SetHeader(name, values.First());
    Assert.Equal([values.First()], _builder.Headers[name]);

    _builder.SetHeader(name, values);
    Assert.Equal(values, _builder.Headers[name]);

    _builder.SetHeader(new HttpHeader(name, values));
    Assert.Equal(values, _builder.Headers[name]);
  }

  [Fact(DisplayName = "SetHeaders: it should set the correct headers.")]
  public void SetHeaders_it_should_set_the_correct_headers()
  {
    HttpHeader[] headers =
    [
      new("Accept", "text/html"),
      new("Content-Type", "application/json")
    ];

    _builder.SetHeaders(headers);
    foreach (HttpHeader header in headers)
    {
      Assert.Equal(header.Values, _builder.Headers[header.Name]);
    }
  }

  [Theory(DisplayName = "SetQuery: it should set the correct method.")]
  [InlineData("POST")]
  public void SetMethod_it_should_set_the_correct_method(string methodString)
  {
    Assert.NotEqual(_builder.Method, methodString);

    _builder.SetMethod(methodString);
    Assert.Equal(methodString, _builder.Method);

    HttpMethod method = new(methodString);
    _builder.SetMethod(method);
    Assert.Equal(methodString, _builder.Method);
  }

  [Theory(DisplayName = "SetUrl: it should set the correct URL.")]
  [InlineData("/people/123")]
  public void SetUrl_it_should_set_the_correct_Url(string urlString)
  {
    _builder.SetUrl(urlString);
    Assert.Equal(urlString, _builder.Url);

    Uri uri = new(urlString, UriKind.RelativeOrAbsolute);
    _builder.SetUrl(uri);
    Assert.Equal(urlString, _builder.Url);
  }

  [Theory(DisplayName = "WithAuthorization: it should set the correct authorization.")]
  [InlineData("Basic", "YWRtaW46UEBzJFcwckQ=")]
  [InlineData("Bearer", "d304ce55-c120-4092-a135-c02f6e2d0d5b")]
  public void WithAuthorization_it_should_set_the_correct_authorization(string scheme, string credentials)
  {
    Assert.Null(_builder.Authorization);

    _builder.WithAuthorization(scheme, credentials);
    Assert.NotNull(_builder.Authorization);
    Assert.Equal(scheme, _builder.Authorization.Scheme);
    Assert.Equal(credentials, _builder.Authorization.Credentials);

    _builder.WithAuthorization(new HttpAuthorization(scheme, credentials));
    Assert.NotNull(_builder.Authorization);
    Assert.Equal(scheme, _builder.Authorization.Scheme);
    Assert.Equal(credentials, _builder.Authorization.Credentials);

    _builder.WithAuthorization(authorization: null);
    Assert.Null(_builder.Authorization);
  }

  [Theory(DisplayName = "WithBasicAuthorization: it should set the correct authorization.")]
  [InlineData("admin", "P@s$W0rD")]
  public void WithBasicAuthorization_it_should_set_the_correct_authorization(string identifier, string secret)
  {
    Assert.Null(_builder.Authorization);

    string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{identifier}:{secret}"));

    _builder.WithBasicAuthorization(identifier, secret);
    Assert.NotNull(_builder.Authorization);
    Assert.Equal(AuthenticationSchemes.Basic, _builder.Authorization.Scheme);
    Assert.Equal(credentials, _builder.Authorization.Credentials);

    _builder.WithBasicAuthorization(new Credentials(identifier, secret));
    Assert.NotNull(_builder.Authorization);
    Assert.Equal(AuthenticationSchemes.Basic, _builder.Authorization.Scheme);
    Assert.Equal(credentials, _builder.Authorization.Credentials);

    _builder.WithBasicAuthorization(credentials);
    Assert.NotNull(_builder.Authorization);
    Assert.Equal(AuthenticationSchemes.Basic, _builder.Authorization.Scheme);
    Assert.Equal(credentials, _builder.Authorization.Credentials);
  }

  [Theory(DisplayName = "WithBearerAuthorization: it should set the correct authorization.")]
  [InlineData("ABC123")]
  public void WithBearerAuthorization_it_should_set_the_correct_authorization(string credentials)
  {
    Assert.Null(_builder.Authorization);

    _builder.WithBearerAuthorization(credentials);
    Assert.NotNull(_builder.Authorization);
    Assert.Equal(AuthenticationSchemes.Bearer, _builder.Authorization.Scheme);
    Assert.Equal(credentials, _builder.Authorization.Credentials);
  }

  [Fact(DisplayName = "WithParameters: it should apply the correct parameters.")]
  public void WithParameters_it_should_apply_the_correct_parameters()
  {
    HttpRequestParameters parameters = new()
    {
      Method = HttpMethod.Put,
      Uri = new("/people/123", UriKind.Relative),
      Content = JsonContent.Create(_person),
      Authorization = HttpAuthorization.Bearer(Guid.NewGuid().ToString())
    };
    parameters.Headers.Add(new HttpHeader("Content-Type", "application/json"));

    _builder.WithParameters(parameters);
    Assert.Equal(parameters.Method.Method, _builder.Method);
    Assert.Equal(parameters.Uri.ToString(), _builder.Url);
    Assert.Same(parameters.Content, _builder.Content);

    Assert.NotNull(_builder.Authorization);
    Assert.Equal(parameters.Authorization.Scheme, _builder.Authorization.Scheme);
    Assert.Equal(parameters.Authorization.Credentials, _builder.Authorization.Credentials);

    foreach (HttpHeader header in parameters.Headers)
    {
      Assert.Contains(_builder.Headers, h => h.Key == header.Name && h.Value.SequenceEqual(header.Values));
    }
  }
}
