namespace Logitar.Net.Http;

internal static class HttpRequestParametersExtensions
{
  public static JsonRequestOptions ToJsonOptions(this HttpRequestOptions options) => new()
  {
    Content = options.Content,
    Headers = options.Headers,
    Authorization = options.Authorization,
    SerializerOptions = IntegrationTestHelper.SerializerOptions
  };
}
