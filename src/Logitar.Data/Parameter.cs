namespace Logitar.Data;

/// <summary>
/// TODO(fpion): document
/// </summary>
public record Parameter : IParameter
{
  public Parameter(string name, object value)
  {
    if (string.IsNullOrWhiteSpace(name))
    {
      throw new ArgumentException("The parameter name is required.", nameof(name));
    }

    Name = name.Trim();
    Value = value;
  }

  public string Name { get; }
  public object Value { get; }
}
