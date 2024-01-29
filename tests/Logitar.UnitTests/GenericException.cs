namespace Logitar;

internal class GenericException<T> : Exception
{
  private const string ErrorMessage = "An unexpected error occurred while handling the specified type.";

  public GenericException(Exception? innerException = null) : base(BuildMessage(), innerException)
  {
  }

  private static string BuildMessage() => new ErrorMessageBuilder(ErrorMessage)
    .AddData("Type", typeof(T).GetNamespaceQualifiedName())
    .Build();
}
