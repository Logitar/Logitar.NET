using Bogus;

namespace Logitar.Net.Mail.SendGrid.Payloads;

[Trait(Traits.Category, Categories.Unit)]
public class PersonalizationPayloadTests
{
  private readonly Faker _faker = new();

  [Fact(DisplayName = "ctor: it should construct the correct payload from a MailMessage.")]
  public void ctor_it_should_construct_the_correct_payload_from_a_MailMessage()
  {
    MailMessage message = new();
    message.To.Add(new MailAddress(_faker.Person.Email, _faker.Person.FullName));
    message.Bcc.Add(new MailAddress(_faker.Internet.Email(), _faker.Name.FullName()));

    PersonalizationPayload payload = new(message);
    Assert.Equal(message.To.Count, payload.To.Count);
    Assert.Null(payload.CC);
    Assert.NotNull(payload.Bcc);
    Assert.Equal(message.Bcc.Count, payload.Bcc.Count);

    foreach (MailAddress recipient in message.To)
    {
      Assert.Contains(payload.To, r => r.Address == recipient.Address && r.DisplayName == recipient.DisplayName);
    }
    foreach (MailAddress recipient in message.Bcc)
    {
      Assert.Contains(payload.Bcc, r => r.Address == recipient.Address && r.DisplayName == recipient.DisplayName);
    }
  }
}
