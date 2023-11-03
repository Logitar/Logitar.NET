namespace Logitar.Data;

/// <summary>
/// Represents a data command.
/// </summary>
internal record Command : ICommand
{
  /// <summary>
  /// Initializes a new instance of the <see cref="Command"/> class.
  /// </summary>
  /// <param name="text">The text of the command.</param>
  /// <param name="parameters">The parameters of the command.</param>
  /// <exception cref="ArgumentException">The command text was missing.</exception>
  public Command(string text, IEnumerable<object> parameters)
  {
    if (string.IsNullOrWhiteSpace(text))
    {
      throw new ArgumentException("The command text is required.", nameof(text));
    }

    Text = text.Trim();
    Parameters = parameters;
  }

  /// <summary>
  /// Gets the text of the command.
  /// </summary>
  public string Text { get; }
  /// <summary>
  /// Gets the parameters of the command.
  /// </summary>
  public IEnumerable<object> Parameters { get; }
}
