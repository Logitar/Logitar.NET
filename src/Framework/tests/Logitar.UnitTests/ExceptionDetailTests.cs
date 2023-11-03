using Bogus;
using System.Collections;

namespace Logitar;

[Trait(Traits.Category, Categories.Unit)]
public class ExceptionDetailTests
{
  private readonly Faker _faker = new();

  [Fact(DisplayName = "From: it should return the correct exception detail.")]
  public void From_it_should_return_the_correct_exception_detail()
  {
    Exception innerException = new("INNER_EXCEPTION");
    Exception exception = new("An unexpected error occurred.", innerException)
    {
      HResult = _faker.Random.Number(0, int.MaxValue),
      HelpLink = $"https://www.{_faker.Internet.DomainName()}",
      Source = nameof(From_it_should_return_the_correct_exception_detail)
    };

    string stackTrace = nameof(exception.StackTrace);
    FieldInfo? _stackTraceString = exception.GetType().GetField("_stackTraceString", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(_stackTraceString);
    _stackTraceString.SetValue(exception, stackTrace);

    MethodBase? targetSite = MethodBase.GetCurrentMethod();
    Assert.NotNull(targetSite);
    FieldInfo? _exceptionMethod = exception.GetType().GetField("_exceptionMethod", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(_exceptionMethod);
    _exceptionMethod.SetValue(exception, targetSite);

    Guid id = Guid.NewGuid();
    exception.Data.Add("Id", id);
    exception.Data.Add("Culture", CultureInfo.CurrentCulture);
    exception.Data.Add(CultureInfo.CurrentCulture, CultureInfo.CurrentCulture.DisplayName);

    ExceptionDetail detail = ExceptionDetail.From(exception);

    Assert.Equal(exception.GetType().AssemblyQualifiedName, detail.Type);
    Assert.Equal(exception.Message, detail.Message);
    Assert.Equal(innerException.Message, detail.InnerException?.Message);
    Assert.Equal(exception.HResult, detail.HResult);
    Assert.Equal(exception.HelpLink, detail.HelpLink);
    Assert.Equal(exception.Source, detail.Source);
    Assert.Equal(stackTrace, detail.StackTrace);
    Assert.Equal(exception.TargetSite?.ToString(), detail.TargetSite);

    DictionaryEntry entry = Assert.Single(detail.Data);
    Assert.Equal("Id", entry.Key);
    Assert.Equal(id, entry.Value);
  }
}
