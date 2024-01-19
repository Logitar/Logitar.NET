using Bogus;

namespace Logitar.Net.Mail.SendGrid.Payloads;

[Trait(Traits.Category, Categories.Unit)]
public class EmailPayloadTests
{
  private readonly Faker _faker = new();

  [Fact(DisplayName = "ctor: it should construct the correct payload from a MailAddress.")]
  public void ctor_it_should_construct_the_correct_payload_from_a_MailAddress()
  {
    MailAddress mail = new(_faker.Person.Email, _faker.Person.FullName);
    EmailPayload email = new(mail);
    Assert.Equal(mail.Address, email.Address);
    Assert.Equal(mail.DisplayName, email.DisplayName);
  }

  [Fact(DisplayName = "ctor: it should construct the correct payload from parameters.")]
  public void ctor_it_should_construct_the_correct_payload_from_parameters()
  {
    EmailPayload email = new(_faker.Person.Email, _faker.Person.FullName);
    Assert.Equal(_faker.Person.Email, email.Address);
    Assert.Equal(_faker.Person.FullName, email.DisplayName);
  }
}
