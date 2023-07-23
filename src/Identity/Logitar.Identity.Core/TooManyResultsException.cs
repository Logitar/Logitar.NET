using Logitar.EventSourcing;

namespace Logitar.Identity.Core;

public class TooManyResultsException : Exception
{
  public TooManyResultsException(Type type, int expected, int actual)
    : base(BuildMessage(type, expected, actual))
  {
    Type = type.GetName();
    Expected = expected;
    Actual = actual;
  }

  public string Type
  {
    get => (string)Data[nameof(Type)]!;
    private set => Data[nameof(Type)] = value;
  }
  public int Expected
  {
    get => (int)Data[nameof(Expected)]!;
    private set => Data[nameof(Expected)] = value;
  }
  public int Actual
  {
    get => (int)Data[nameof(Actual)]!;
    private set => Data[nameof(Actual)] = value;
  }

  private static string BuildMessage(Type type, int expected, int actual)
  {
    StringBuilder message = new();

    message.AppendLine("There are too many results.");
    message.Append("Type: ").AppendLine(type.GetName());
    message.Append("Expected: ").Append(expected).AppendLine();
    message.Append("Actual: ").Append(actual).AppendLine();

    return message.ToString();
  }
}

public class TooManyResultsException<T> : TooManyResultsException
{
  public TooManyResultsException(int expected, int actual) : base(typeof(T), expected, actual)
  {
  }
}
