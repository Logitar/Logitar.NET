namespace Logitar.Net.Http;

public record Person
{
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string FullName => $"{FirstName} {LastName}";

  public DateTime DateOfBirth { get; set; }
  public byte Age
  {
    get
    {
      TimeSpan difference = DateTime.Now - DateOfBirth;
      return (byte)(difference.TotalDays / 365.2425);
    }
  }

  public Person() : this(string.Empty, string.Empty, default)
  {
  }

  public Person(string firstName, string lastName, DateTime dateOfBirth)
  {
    FirstName = firstName;
    LastName = lastName;
    DateOfBirth = dateOfBirth;
  }
}
