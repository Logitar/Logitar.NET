namespace Logitar.Data;

internal class UpdateBuilderMock : UpdateBuilder
{
  public UpdateBuilderMock() : base()
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
