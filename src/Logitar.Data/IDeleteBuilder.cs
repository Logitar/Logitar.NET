namespace Logitar.Data;

/// <summary>
/// Represents a delete data command builder. It exposes methods to configure and build a delete data command.
/// </summary>
public interface IDeleteBuilder : ICommandBuilder
{
  /// <summary>
  /// Applies the specified conditions to the command builder.
  /// </summary>
  /// <param name="conditions">The conditions to apply.</param>
  /// <returns>The command builder.</returns>
  IDeleteBuilder Where(params Condition[] conditions);
}
