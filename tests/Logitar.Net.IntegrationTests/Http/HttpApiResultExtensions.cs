using Logitar.Demo.Models.Test;

namespace Logitar.Net.Http;

internal static class HttpApiResultExtensions
{
  public static void AssertIsValid(this HttpApiResult result, HttpRequestParameters parameters, IHttpApiSettings? settings = null)
  {
    result.AssertIsValid(parameters, contentValue: null, settings);
  }
  public static void AssertIsValid(this HttpApiResult result, HttpRequestParameters parameters, object? contentValue, IHttpApiSettings? settings = null)
  {
    Assert.Equal(HttpStatusCode.OK, result.Status.Value);

    JsonApiResult? jsonResult = result as JsonApiResult;
    Assert.NotNull(jsonResult);

    jsonResult.AssertIsValid(parameters, contentValue, settings);
  }

  public static void AssertIsValid(this JsonApiResult result, HttpRequestParameters parameters, IHttpApiSettings? settings = null)
  {
    result.AssertIsValid(parameters, contentValue: null, settings);
  }
  public static void AssertIsValid(this JsonApiResult result, HttpRequestParameters parameters, object? contentValue, IHttpApiSettings? settings = null)
  {
    Assert.Equal(HttpStatusCode.OK, result.Status.Value);

    HttpRequestModel? request = result.Deserialize<HttpRequestModel>(IntegrationTestHelper.SerializerOptions);
    Assert.NotNull(request);

    request.AssertIsValid(parameters, contentValue, settings);
  }

  public static void AssertIsValid(this HttpRequestModel request, HttpRequestParameters parameters, IHttpApiSettings? settings = null)
  {
    request.AssertIsValid(parameters, contentValue: null, settings);
  }
  public static void AssertIsValid(this HttpRequestModel request, HttpRequestParameters parameters, object? contentValue, IHttpApiSettings? settings = null)
  {
    Assert.Equal(parameters.Method.Method, request.Method);
    Assert.EndsWith(parameters.Uri.ToString(), request.Url);

    if (contentValue == null)
    {
      Assert.Null(request.Content);
    }
    else
    {
      string jsonContent = JsonSerializer.Serialize(contentValue, contentValue.GetType(), IntegrationTestHelper.SerializerOptions);
      Assert.Equal(jsonContent, request.Content);
    }

    foreach (HttpHeader header in parameters.Headers)
    {
      Assert.Contains(request.Headers, h => h.Name == header.Name && h.Values.SequenceEqual(header.Values));
    }

    IHttpAuthorization? authorization = parameters.Authorization;
    if (settings != null)
    {
      authorization ??= settings.Authorization;

      if (settings.BaseUri != null)
      {
        Assert.StartsWith(settings.BaseUri.ToString(), request.Url);
      }

      foreach (HttpHeader header in settings.Headers)
      {
        Assert.Contains(request.Headers, h => h.Name == header.Name && h.Values.SequenceEqual(header.Values));
      }
    }

    Assert.Equal(authorization, request.Authorization);
  }
}
