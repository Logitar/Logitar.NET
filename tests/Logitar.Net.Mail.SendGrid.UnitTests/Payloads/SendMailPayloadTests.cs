using Bogus;

namespace Logitar.Net.Mail.SendGrid.Payloads;

[Trait(Traits.Category, Categories.Unit)]
public class SendMailPayloadTests
{
  private readonly Faker _faker = new();

  [Fact(DisplayName = "ctor: it should construct the correct payload from a MailMessage.")]
  public void ctor_it_should_construct_the_correct_payload_from_a_MailMessage()
  {
    MailAddress from = new(_faker.Internet.Email(), _faker.Name.FullName());
    MailAddress sender = new(_faker.Internet.Email(), _faker.Name.FullName());
    MailAddress replyTo = new(_faker.Internet.Email(), _faker.Name.FullName());
    MailAddress to = new(_faker.Person.Email, _faker.Person.FullName);
    MailMessage message = new(from, to)
    {
      Sender = sender,
      Subject = "Test",
      Body = "<p>Hello World!</p>",
      IsBodyHtml = true,
#pragma warning disable 0618
      // NOTE(fpion): the MailMessage.ReplyTo property is marked as obsolete, but it is still supported by SendGrid.
      ReplyTo = replyTo,
#pragma warning restore 0618
    };
    message.ReplyToList.Add(replyTo);

    SendMailPayload payload = new(message);

    PersonalizationPayload personalization = Assert.Single(payload.Personalizations);
    EmailPayload recipient = Assert.Single(personalization.To);
    Assert.Equal(to.Address, recipient.Address);
    Assert.Equal(to.DisplayName, recipient.DisplayName);
    Assert.Null(personalization.CC);
    Assert.Null(personalization.Bcc);

    Assert.Equal(sender.Address, payload.From.Address);
    Assert.Equal(sender.DisplayName, payload.From.DisplayName);

    Assert.NotNull(payload.ReplyTo);
    Assert.Equal(replyTo.Address, payload.ReplyTo.Address);
    Assert.Equal(replyTo.DisplayName, payload.ReplyTo.DisplayName);

    Assert.NotNull(payload.ReplyToList);
    EmailPayload replyToPayload = Assert.Single(payload.ReplyToList);
    Assert.Equal(replyTo.Address, replyToPayload.Address);
    Assert.Equal(replyTo.DisplayName, replyToPayload.DisplayName);

    Assert.Equal(message.Subject, payload.Subject);

    ContentPayload content = Assert.Single(payload.Contents);
    Assert.Equal(MediaTypeNames.Text.Html, content.Type);
    Assert.Equal(message.Body, content.Value);
  }

  [Fact(DisplayName = "ctor: it should throw ArgumentException when the message does not provide a sender nor a from email.")]
  public void ctor_it_should_throw_ArgumentException_when_the_message_does_not_provide_a_sender_nor_a_from_email()
  {
    var exception = Assert.Throws<ArgumentException>(() => new SendMailPayload(new MailMessage()));
    Assert.StartsWith("At least one of the following must be provided: From, Sender.", exception.Message);
    Assert.Equal("message", exception.ParamName);
  }
}
