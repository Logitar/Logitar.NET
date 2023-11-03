namespace Logitar.Data;

/// <summary>
/// Represents an update data command builder. It exposes methods to configure and build an update data command.
/// </summary>
public interface IUpdateBuilder : ICommandBuilder
{
  /// <summary>
  /// Applies the specified column updates to the command builder.
  /// </summary>
  /// <param name="updates">The column updates to apply.</param>
  /// <returns>The command builder.</returns>
  IUpdateBuilder Set(params Update[] updates);

  /// <summary>
  /// Applies the specified conditions to the command builder.
  /// </summary>
  /// <param name="conditions">The conditions to apply.</param>
  /// <returns>The command builder.</returns>
  IUpdateBuilder Where(params Condition[] conditions);
}
