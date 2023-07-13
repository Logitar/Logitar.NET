using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Logitar.Security.Cryptography;

[Trait(Traits.Category, Categories.Unit)]
public class Pbkdf2Tests
{
  private const string Password = "P@s$W0rD";

  private readonly Pbkdf2 _pbkdf2 = new(Password);

  [Fact(DisplayName = "It should be constructed correctly using bytes password.")]
  public void It_should_be_constructed_correctly_using_bytes_password()
  {
    byte[] password = RandomNumberGenerator.GetBytes(20);
    Pbkdf2 pbkdf2 = new(password);

    KeyDerivationPrf algorithm = GetAlgorithm(pbkdf2);
    Assert.Equal(KeyDerivationPrf.HMACSHA256, algorithm);

    int iterations = GetIterations(pbkdf2);
    Assert.Equal(600000, iterations);

    byte[] salt = GetSalt(pbkdf2);
    Assert.Equal(32, salt.Length);

    byte[] hash = KeyDerivation.Pbkdf2(Convert.ToBase64String(password), salt, algorithm, iterations, salt.Length);
    Assert.Equal(hash, GetHash(pbkdf2));
  }
  [Fact(DisplayName = "It should be constructed correctly using string password.")]
  public void It_should_be_constructed_correctly_using_string_password()
  {
    Pbkdf2 pbkdf2 = new(Password);

    KeyDerivationPrf algorithm = GetAlgorithm(pbkdf2);
    Assert.Equal(KeyDerivationPrf.HMACSHA256, algorithm);

    int iterations = GetIterations(pbkdf2);
    Assert.Equal(600000, iterations);

    byte[] salt = GetSalt(pbkdf2);
    Assert.Equal(32, salt.Length);

    byte[] hash = KeyDerivation.Pbkdf2(Password, salt, algorithm, iterations, salt.Length);
    Assert.Equal(hash, GetHash(pbkdf2));
  }

  [Fact(DisplayName = "It should be equal when compared to an equivalent PBKDF2.")]
  public void It_should_be_equal_when_compared_to_an_equivalent_PBKDF2()
  {
    Pbkdf2 other = Pbkdf2.Parse(_pbkdf2.ToString());
    Assert.True(_pbkdf2.Equals(other));
  }

  [Fact(DisplayName = "It should not be equal when compared to a different PBKDF2.")]
  public void It_should_not_be_equal_when_compared_to_a_different_PBKDF2()
  {
    Pbkdf2 other = new(Password[1..]);
    Assert.False(_pbkdf2.Equals(other));
  }

  [Fact(DisplayName = "It should not be equal when compared to an object of another type.")]
  public void It_should_not_be_equal_when_compared_to_an_object_of_another_type()
  {
    Assert.False(_pbkdf2.Equals(Password));
  }

  [Fact(DisplayName = "It should not be equal when compared to null.")]
  public void It_should_not_be_equal_when_compared_to_null()
  {
    Assert.False(_pbkdf2.Equals(null));
  }

  [Fact(DisplayName = "It should parse a valid string representation.")]
  public void It_should_parse_a_valid_string_representation()
  {
    Pbkdf2 pbkdf2 = Pbkdf2.Parse(_pbkdf2.ToString());
    Assert.Equal(_pbkdf2, pbkdf2);
  }

  [Fact(DisplayName = "It should return the correct hash code.")]
  public void It_should_return_the_correct_hash_code()
  {
    int hashCode = HashCode.Combine(GetAlgorithm(_pbkdf2), GetIterations(_pbkdf2),
      Convert.ToBase64String(GetSalt(_pbkdf2)), Convert.ToBase64String(GetHash(_pbkdf2)));
    Assert.Equal(hashCode, _pbkdf2.GetHashCode());
  }

  [Fact(DisplayName = "It should return the correct string representation.")]
  public void It_should_return_the_correct_string_representation()
  {
    string s = string.Join(Pbkdf2.Separator, GetAlgorithm(_pbkdf2), GetIterations(_pbkdf2),
      Convert.ToBase64String(GetSalt(_pbkdf2)), Convert.ToBase64String(GetHash(_pbkdf2)));
    Assert.Equal(s, _pbkdf2.ToString());
  }

  [Fact(DisplayName = "It should return false when bytes password is not a match.")]
  public void It_should_return_false_when_bytes_password_is_not_a_match()
  {
    byte[] password = RandomNumberGenerator.GetBytes(20);
    Pbkdf2 pbkdf2 = new(password);
    Assert.False(pbkdf2.IsMatch(password.Skip(1).ToArray()));
  }
  [Fact(DisplayName = "It should return true when bytes password is a match.")]
  public void It_should_return_true_when_bytes_password_is_a_match()
  {
    byte[] password = RandomNumberGenerator.GetBytes(20);
    Pbkdf2 pbkdf2 = new(password);
    Assert.True(pbkdf2.IsMatch(password));
  }

  [Fact(DisplayName = "It should return false when string password is not a match.")]
  public void It_should_return_false_when_string_password_is_not_a_match()
  {
    Assert.True(_pbkdf2.IsMatch(Password));
  }
  [Fact(DisplayName = "It should return true when string password is a match.")]
  public void It_should_return_true_when_string_password_is_a_match()
  {
    Assert.False(_pbkdf2.IsMatch(Password[1..]));
  }

  [Fact(DisplayName = "It should return false when string representation is not valid.")]
  public void It_should_return_false_when_string_representation_is_not_valid()
  {
    Assert.False(Pbkdf2.TryParse(string.Empty, out Pbkdf2? pbkdf2));
    Assert.Null(pbkdf2);
  }

  [Fact(DisplayName = "It should return true when parsing a valid string representation.")]
  public void It_should_return_true_when_parsing_a_valid_string_representation()
  {
    Assert.True(Pbkdf2.TryParse(_pbkdf2.ToString(), out Pbkdf2? pbkdf2));
    Assert.NotNull(pbkdf2);
    Assert.Equal(_pbkdf2, pbkdf2);
  }

  [Fact(DisplayName = "It should throw ArgumentException when string representation is not valid.")]
  public void It_should_throw_ArgumentException_when_string_representation_is_not_valid()
  {
    var exception = Assert.Throws<ArgumentException>(() => Pbkdf2.Parse(string.Empty));
    Assert.Equal("s", exception.ParamName);
  }

  private static KeyDerivationPrf GetAlgorithm(Pbkdf2 pbkdf2)
  {
    FieldInfo? field = typeof(Pbkdf2).GetField("_algorithm", BindingFlags.Instance | BindingFlags.NonPublic);
    Assert.NotNull(field);

    KeyDerivationPrf? algorithm = field.GetValue(pbkdf2) as KeyDerivationPrf?;
    Assert.NotNull(algorithm);

    return algorithm!.Value;
  }
  private static int GetIterations(Pbkdf2 pbkdf2)
  {
    FieldInfo? field = typeof(Pbkdf2).GetField("_iterations", BindingFlags.Instance | BindingFlags.NonPublic);
    Assert.NotNull(field);

    int? iterations = field.GetValue(pbkdf2) as int?;
    Assert.NotNull(iterations);

    return iterations!.Value;
  }
  private static byte[] GetSalt(Pbkdf2 pbkdf2)
  {
    FieldInfo? field = typeof(Pbkdf2).GetField("_salt", BindingFlags.Instance | BindingFlags.NonPublic);
    Assert.NotNull(field);

    byte[]? salt = field.GetValue(pbkdf2) as byte[];
    Assert.NotNull(salt);

    return salt;
  }
  private static byte[] GetHash(Pbkdf2 pbkdf2)
  {
    FieldInfo? field = typeof(Pbkdf2).GetField("_hash", BindingFlags.Instance | BindingFlags.NonPublic);
    Assert.NotNull(field);

    byte[]? hash = field.GetValue(pbkdf2) as byte[];
    Assert.NotNull(hash);

    return hash;
  }
}
