namespace Logitar.Net.Mail.SendGrid.Payloads;

[Trait(Traits.Category, Categories.Unit)]
public class ContentPayloadTests
{
  [Fact(DisplayName = "ctor: it should construct the correct payload from a MailMessage.")]
  public void ctor_it_should_construct_the_correct_payload_from_a_MailMessage()
  {
    MailMessage message = new()
    {
      Body = "<p>Hello World!</p>",
      IsBodyHtml = true
    };
    ContentPayload content = new(message);
    Assert.Equal(MediaTypeNames.Text.Html, content.Type);
    Assert.Equal(message.Body, content.Value);
  }

  [Fact(DisplayName = "ctor: it should construct the correct payload from parameters.")]
  public void ctor_it_should_construct_the_correct_payload_from_parameters()
  {
    string type = MediaTypeNames.Text.Plain;
    string value = "Hello World!";
    ContentPayload content = new(type, value);
    Assert.Equal(type, content.Type);
    Assert.Equal(value, content.Value);
  }
}
