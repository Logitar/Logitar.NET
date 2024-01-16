﻿using Bogus;
using Logitar.Demo.Models.Request;
using Logitar.Net.Http.Settings;

namespace Logitar.Net.Http;

[Trait(Traits.Category, Categories.Integration)]
public class EmptyJsonApiClientExtensionsTests : IDisposable
{
  private const string BaseUrl = "http://localhost:22277";
  private const string Path = "/request";
  private static string RequestUrl => new UriBuilder(BaseUrl) { Path = Path }.ToString();

  private static readonly CancellationToken _cancellationToken = default;
  private static readonly Uri _requestUri = new(Path, UriKind.Relative);

  private readonly Faker _faker = new();
  private readonly Person _person;

  private readonly JsonApiSettings _settings;
  private readonly JsonApiClient _client;

  public EmptyJsonApiClientExtensionsTests()
  {
    _person = new(_faker.Person.FirstName, _faker.Person.LastName, _faker.Person.DateOfBirth);

    _settings = new()
    {
      BaseUri = new Uri(BaseUrl, UriKind.Absolute),
      SerializerOptions = new()
      {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
      }
    };

    _client = new(_settings);
  }

  public void Dispose()
  {
    _client.Dispose();
    GC.SuppressFinalize(this);
  }

  [Fact(DisplayName = "DeleteAsync: it should send the correct request with a cancellation token.")]
  public async Task DeleteAsync_it_should_send_the_correct_request_with_a_cancellation_token()
  {
    JsonApiResponse<Empty> response = await _client.DeleteAsync(_requestUri, _cancellationToken);

    AssertRequest(HttpMethod.Delete, response);
  }

  [Fact(DisplayName = "DeleteAsync: it should send the correct request with a request context.")]
  public async Task DeleteAsync_it_should_send_the_correct_request_with_a_request_context()
  {
    HttpRequestContext context = new()
    {
      Authorization = new AuthorizationSettings
      {
        Scheme = "Bearer",
        Credentials = Guid.NewGuid().ToString()
      }
    };

    JsonApiResponse<Empty> response = await _client.DeleteAsync(_requestUri, context);

    AssertRequest(HttpMethod.Delete, response, context.Authorization);
  }

  [Fact(DisplayName = "GetAsync: it should send the correct request with a cancellation token.")]
  public async Task GetAsync_it_should_send_the_correct_request_with_a_cancellation_token()
  {
    JsonApiResponse<Empty> response = await _client.GetAsync(_requestUri, _cancellationToken);

    AssertRequest(HttpMethod.Get, response);
  }

  [Fact(DisplayName = "GetAsync: it should send the correct request with a request context.")]
  public async Task GetAsync_it_should_send_the_correct_request_with_a_request_context()
  {
    HttpRequestContext context = new()
    {
      Authorization = new AuthorizationSettings
      {
        Scheme = "Bearer",
        Credentials = Guid.NewGuid().ToString()
      }
    };

    JsonApiResponse<Empty> response = await _client.GetAsync(_requestUri, context);

    AssertRequest(HttpMethod.Get, response, context.Authorization);
  }

  [Fact(DisplayName = "PatchAsync: it should send the correct request with a body and a cancellation token.")]
  public async Task PatchAsync_it_should_send_the_correct_request_with_a_body_and_a_cancellation_token()
  {
    JsonApiResponse<Empty> response = await _client.PatchAsync(_requestUri, _person, _cancellationToken);

    AssertRequest(HttpMethod.Patch, _person, response);
  }

  [Fact(DisplayName = "PatchAsync: it should send the correct request with a body and a request context.")]
  public async Task PatchAsync_it_should_send_the_correct_request_with_a_body_and_a_request_context()
  {
    HttpRequestContext context = new()
    {
      Authorization = new AuthorizationSettings
      {
        Scheme = "Bearer",
        Credentials = Guid.NewGuid().ToString()
      }
    };

    JsonApiResponse<Empty> response = await _client.PatchAsync(_requestUri, _person, context);

    AssertRequest(HttpMethod.Patch, _person, response, context.Authorization);
  }

  [Fact(DisplayName = "PatchAsync: it should send the correct request with a cancellation token.")]
  public async Task PatchAsync_it_should_send_the_correct_request_with_a_cancellation_token()
  {
    JsonApiResponse<Empty> response = await _client.PatchAsync(_requestUri, _cancellationToken);

    AssertRequest(HttpMethod.Patch, response);
  }

  [Fact(DisplayName = "PatchAsync: it should send the correct request with a request context.")]
  public async Task PatchAsync_it_should_send_the_correct_request_with_a_request_context()
  {
    HttpRequestContext context = new()
    {
      Authorization = new AuthorizationSettings
      {
        Scheme = "Bearer",
        Credentials = Guid.NewGuid().ToString()
      }
    };

    JsonApiResponse<Empty> response = await _client.PatchAsync(_requestUri, context);

    AssertRequest(HttpMethod.Patch, response, context.Authorization);
  }

  [Fact(DisplayName = "PostAsync: it should send the correct request with a body and a cancellation token.")]
  public async Task PostAsync_it_should_send_the_correct_request_with_a_body_and_a_cancellation_token()
  {
    JsonApiResponse<Empty> response = await _client.PostAsync(_requestUri, _person, _cancellationToken);

    AssertRequest(HttpMethod.Post, _person, response);
  }

  [Fact(DisplayName = "PostAsync: it should send the correct request with a body and a request context.")]
  public async Task PostAsync_it_should_send_the_correct_request_with_a_body_and_a_request_context()
  {
    HttpRequestContext context = new()
    {
      Authorization = new AuthorizationSettings
      {
        Scheme = "Bearer",
        Credentials = Guid.NewGuid().ToString()
      }
    };

    JsonApiResponse<Empty> response = await _client.PostAsync(_requestUri, _person, context);

    AssertRequest(HttpMethod.Post, _person, response, context.Authorization);
  }

  [Fact(DisplayName = "PostAsync: it should send the correct request with a cancellation token.")]
  public async Task PostAsync_it_should_send_the_correct_request_with_a_cancellation_token()
  {
    JsonApiResponse<Empty> response = await _client.PostAsync(_requestUri, _cancellationToken);

    AssertRequest(HttpMethod.Post, response);
  }

  [Fact(DisplayName = "PostAsync: it should send the correct request with a request context.")]
  public async Task PostAsync_it_should_send_the_correct_request_with_a_request_context()
  {
    HttpRequestContext context = new()
    {
      Authorization = new AuthorizationSettings
      {
        Scheme = "Bearer",
        Credentials = Guid.NewGuid().ToString()
      }
    };

    JsonApiResponse<Empty> response = await _client.PostAsync(_requestUri, context);

    AssertRequest(HttpMethod.Post, response, context.Authorization);
  }

  [Fact(DisplayName = "PutAsync: it should send the correct request with a body and a cancellation token.")]
  public async Task PutAsync_it_should_send_the_correct_request_with_a_body_and_a_cancellation_token()
  {
    JsonApiResponse<Empty> response = await _client.PutAsync(_requestUri, _person, _cancellationToken);

    AssertRequest(HttpMethod.Put, _person, response);
  }

  [Fact(DisplayName = "PutAsync: it should send the correct request with a body and a request context.")]
  public async Task PutAsync_it_should_send_the_correct_request_with_a_body_and_a_request_context()
  {
    HttpRequestContext context = new()
    {
      Authorization = new AuthorizationSettings
      {
        Scheme = "Bearer",
        Credentials = Guid.NewGuid().ToString()
      }
    };

    JsonApiResponse<Empty> response = await _client.PutAsync(_requestUri, _person, context);

    AssertRequest(HttpMethod.Put, _person, response, context.Authorization);
  }

  [Fact(DisplayName = "PutAsync: it should send the correct request with a cancellation token.")]
  public async Task PutAsync_it_should_send_the_correct_request_with_a_cancellation_token()
  {
    JsonApiResponse<Empty> response = await _client.PutAsync(_requestUri, _cancellationToken);

    AssertRequest(HttpMethod.Put, response);
  }

  [Fact(DisplayName = "PutAsync: it should send the correct request with a request context.")]
  public async Task PutAsync_it_should_send_the_correct_request_with_a_request_context()
  {
    HttpRequestContext context = new()
    {
      Authorization = new AuthorizationSettings
      {
        Scheme = "Bearer",
        Credentials = Guid.NewGuid().ToString()
      }
    };

    JsonApiResponse<Empty> response = await _client.PutAsync(_requestUri, context);

    AssertRequest(HttpMethod.Put, response, context.Authorization);
  }

  private void AssertRequest(HttpMethod method, JsonApiResponse<Empty> response)
  {
    AssertRequest(method, requestContent: null, response);
  }
  private void AssertRequest(HttpMethod method, JsonApiResponse<Empty> response, IAuthorizationSettings? authorizationSettings)
  {
    AssertRequest(method, requestContent: null, response, authorizationSettings);
  }
  private void AssertRequest(HttpMethod method, object? requestContent, JsonApiResponse<Empty> response)
  {
    AssertRequest(method, requestContent, response, authorizationSettings: null);
  }
  private void AssertRequest(HttpMethod method, object? requestContent, JsonApiResponse<Empty> response, IAuthorizationSettings? authorizationSettings)
  {
    Assert.Equal(HttpStatusCode.OK, response.Status.Value);

    Assert.Equal(default, response.Value);

    Assert.NotNull(response.ContentText);
    HttpRequestModel? request = JsonSerializer.Deserialize<HttpRequestModel>(response.ContentText, _settings.SerializerOptions);
    Assert.NotNull(request);

    if (requestContent == null)
    {
      Assert.Null(request.Content);
    }
    else
    {
      string json = JsonSerializer.Serialize(requestContent, _settings.SerializerOptions);
      Assert.Equal(json, request.Content);
    }

    Assert.Equal(method.Method, request.Method);

    Assert.Equal(RequestUrl, request.Url);

    authorizationSettings ??= _settings.Authorization;
    if (authorizationSettings != null)
    {
      Assert.Contains(request.Headers, h => h.Name == "Authorization" && h.Values.Single() == $"{authorizationSettings.Scheme} {authorizationSettings.Credentials}");
    }

    foreach (HttpHeader header in _settings.Headers)
    {
      Assert.Contains(request.Headers, h => h.Name == header.Name && h.Values.SequenceEqual(header.Values));
    }
  }
}
