namespace Logitar.Data;

/// <summary>
/// Represents a data command builder. It exposes methods to configure and build a data command.
/// </summary>
public interface ICommandBuilder
{
  /// <summary>
  /// Builds the data command.
  /// </summary>
  /// <returns>The data command.</returns>
  ICommand Build();
}
