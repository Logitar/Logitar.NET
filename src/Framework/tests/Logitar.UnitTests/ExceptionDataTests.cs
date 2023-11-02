using Bogus;
using System.Reflection;

namespace Logitar;

[Trait(Traits.Category, Categories.Unit)]
public class ExceptionDataTests
{
  private readonly Faker _faker = new();

  [Fact(DisplayName = "From: it should return the correct exception data.")]
  public void From_it_should_return_the_correct_exception_data()
  {
    Exception innerException = new("INNER_EXCEPTION");
    Exception exception = new("An unexpected error occurred.", innerException)
    {
      HResult = _faker.Random.Number(0, int.MaxValue),
      HelpLink = $"https://www.{_faker.Internet.DomainName()}",
      Source = nameof(From_it_should_return_the_correct_exception_data)
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

    // TODO(fpion): Data

    ExceptionData data = ExceptionData.From(exception);

    Assert.Equal(exception.GetType().AssemblyQualifiedName, data.Type);
    Assert.Equal(exception.Message, data.Message);
    Assert.Equal(innerException.Message, data.InnerException?.Message);
    Assert.Equal(exception.HResult, data.HResult);
    Assert.Equal(exception.HelpLink, data.HelpLink);
    Assert.Equal(exception.Source, data.Source);
    Assert.Equal(stackTrace, data.StackTrace);
    Assert.Equal(exception.TargetSite?.ToString(), data.TargetSite);
    // TODO(fpion): Data
  }
}
