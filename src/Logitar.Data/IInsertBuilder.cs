namespace Logitar.Data;

/// <summary>
/// Represents an insert data command builder. It exposes methods to configure and build an insert data command.
/// </summary>
public interface IInsertBuilder : ICommandBuilder
{
  /// <summary>
  /// Inserts the specified row values in the command builder.
  /// </summary>
  /// <param name="values">The row values.</param>
  /// <returns>The command builder.</returns>
  IInsertBuilder Value(params object?[] values);
}
