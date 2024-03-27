namespace Logitar.Net.Sms;

[Trait(Traits.Category, Categories.Unit)]
public class SmsMessageTests
{
  private const string From = "+15148454636";
  private const string To = "(514) 872-0311";
  private const string Body = "  Hello World!  ";

  [Fact(DisplayName = "ctor: it should create a new SMS message.")]
  public void ctor_it_should_create_a_new_Sms_message()
  {
    SmsMessage message = new(From, To, Body);
    Assert.Equal(From, message.From);
    Assert.Equal(string.Concat("+1", new string(To.Where(char.IsDigit).ToArray())), message.To);
    Assert.Equal(Body.Trim(), message.Body);
  }

  [Theory(DisplayName = "ctor: it should throw ArgumentException when the body is missing.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("    ")]
  public void ctor_it_should_throw_ArgumentException_when_the_body_is_missing(string? body)
  {
    var exception = Assert.Throws<ArgumentException>(() => new SmsMessage(From, To, body!));
    Assert.Equal("body", exception.ParamName);
    Assert.StartsWith("The text content of the message is required.", exception.Message);
  }

  [Theory(DisplayName = "ctor: it should throw ArgumentException when the recipient number is not valid.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("    ")]
  [InlineData("123456")]
  [InlineData("+1234567890987654321")]
  [InlineData("ABCDEFGHIJ")]
  public void ctor_it_should_throw_ArgumentException_when_the_recipient_number_is_not_valid(string? to)
  {
    var exception = Assert.Throws<ArgumentException>(() => new SmsMessage(From, to!, Body));
    Assert.Equal("to", exception.ParamName);
    Assert.StartsWith($"The value '{to}' is not a valid phone number.", exception.Message);
  }

  [Theory(DisplayName = "ctor: it should throw ArgumentException when the sender number is not valid.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("    ")]
  [InlineData("123456")]
  [InlineData("+1234567890987654321")]
  [InlineData("ABCDEFGHIJ")]
  public void ctor_it_should_throw_ArgumentException_when_the_sender_number_is_not_valid(string? from)
  {
    var exception = Assert.Throws<ArgumentException>(() => new SmsMessage(from!, To, Body));
    Assert.Equal("from", exception.ParamName);
    Assert.StartsWith($"The value '{from}' is not a valid phone number.", exception.Message);
  }

  [Fact(DisplayName = "ctor: it should throw ArgumentOutOfRangeException when the body is too long.")]
  public void ctor_it_should_throw_ArgumentOutOfRangeException_when_the_body_is_too_long()
  {
    Random random = new();
    char[] bodyChars = new char[SmsMessage.MaximumLength + 1];
    for (int i = 0; i < bodyChars.Length; i++)
    {
      bodyChars[i] = (char)random.Next(32, 126); // NOTE(fpion): all characters in the ASCII table between ' ' and '~', including letters and digits.
    }
    string body = new(bodyChars);

    var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new SmsMessage(From, To, body));
    Assert.Equal("body", exception.ParamName);
    Assert.StartsWith("The text content of the message must not exceed 1600 characters.", exception.Message);
  }
}
