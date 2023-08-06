namespace Logitar.Data;

/// <summary>
/// Represents a generic SQL update command builder, to be used by specific implementations.
/// </summary>
public class UpdateBuilder : SqlBuilder, IUpdateBuilder
{
  /// <summary>
  /// Gets the list of conditions of the command.
  /// </summary>
  protected ICollection<Condition> Conditions { get; } = new List<Condition>();
  /// <summary>
  /// Gets the list of column updates of the command.
  /// </summary>
  protected ICollection<Update> Updates { get; } = new List<Update>();

  /// <summary>
  /// Applies the specified column updates to the command builder.
  /// </summary>
  /// <param name="updates">The column updates to apply.</param>
  /// <returns>The command builder.</returns>
  public IUpdateBuilder Set(params Update[] updates)
  {
    Updates.AddRange(updates);
    return this;
  }

  /// <summary>
  /// Applies the specified conditions to the command.
  /// </summary>
  /// <param name="conditions">The conditions to apply.</param>
  /// <returns>The command builder.</returns>
  public IUpdateBuilder Where(params Condition[] conditions)
  {
    Conditions.AddRange(conditions);
    return this;
  }

  /// <summary>
  /// Builds the update data command.
  /// </summary>
  /// <returns>The data command.</returns>
  public ICommand Build()
  {
    if (!Updates.Any())
    {
      throw new InvalidOperationException("At least one column must be updated.");
    }

    IEnumerable<string> formattedTables = Updates.Where(x => x.Column.Table != null)
      .Select(x => Format(x.Column.Table!))
      .Distinct();
    if (formattedTables.Count() > 1)
    {
      throw new InvalidOperationException("An update command cannot update multiple tables.");
    }
    string formattedTable = formattedTables.SingleOrDefault()
      ?? throw new InvalidOperationException("At least one column must specify a table.");

    StringBuilder text = new();

    text.Append(Dialect.UpdateClause).Append(' ').AppendLine(formattedTable);

    text.Append(Dialect.SetClause).Append(' ').AppendLine(string.Join(", ", Updates.Select(Format)));

    if (Conditions.Any())
    {
      _ = Dialect.GroupOperators.TryGetValue("AND", out string? andOperator);
      andOperator ??= "AND";

      text.Append(Dialect.WhereClause).Append(' ')
        .AppendLine(string.Join($" {andOperator} ", Conditions.Select(Format)));
    }

    IEnumerable<object> parameters = Parameters.Select(Dialect.CreateParameter);

    return new Command(text.ToString().TrimEnd(','), parameters);
  }
}
