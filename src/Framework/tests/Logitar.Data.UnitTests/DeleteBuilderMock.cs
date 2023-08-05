namespace Logitar.Data;

internal class DeleteBuilderMock : DeleteBuilder
{
  public DeleteBuilderMock(TableId source) : base(source)
  {
    Dialect.ComparisonOperators["="] = "==";
    Dialect.ComparisonOperators[">"] = ">>";
    Dialect.ComparisonOperators[">="] = ">>==";
    Dialect.ComparisonOperators["<"] = "<<";
    Dialect.ComparisonOperators["<="] = "<<==";
    Dialect.ComparisonOperators["<>"] = "!=";

    Dialect.GroupOperators["AND"] = "ET";
    Dialect.GroupOperators["OR"] = "OU";
  }

  public override Dialect Dialect { get; set; } = new DialectMock();
}
