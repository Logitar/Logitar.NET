namespace Logitar.Data;

/// <summary>
/// Represents an abstraction of a generic SQL delete command builder, to be used by specific implementations.
/// </summary>
public abstract class DeleteBuilder : SqlBuilder, IDeleteBuilder
{
  /// <summary>
  /// Initializes a new instance of the <see cref="DeleteBuilder"/> class.
  /// </summary>
  /// <param name="source">The source table.</param>
  /// <exception cref="ArgumentException">The source table name has not been specified.</exception>
  protected DeleteBuilder(TableId source)
  {
    if (source.Table == null)
    {
      throw new ArgumentException("The table name is required.", nameof(source));
    }

    Source = source;
  }

  /// <summary>
  /// Gets the table in which the command will delete data from.
  /// </summary>
  protected TableId Source { get; }
  /// <summary>
  /// Gets the list of conditions of the command.
  /// </summary>
  protected ICollection<Condition> Conditions { get; } = new List<Condition>();

  /// <summary>
  /// Applies the specified conditions to the command.
  /// </summary>
  /// <param name="conditions">The conditions to apply.</param>
  /// <returns>The command builder.</returns>
  public IDeleteBuilder Where(params Condition[] conditions)
  {
    Conditions.AddRange(conditions);
    return this;
  }

  /// <summary>
  /// Builds the delete data command.
  /// </summary>
  /// <returns>The data command.</returns>
  public ICommand Build()
  {
    StringBuilder text = new();

    text.Append(Dialect.DeleteFromClause).Append(' ').AppendLine(Format(Source));

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
