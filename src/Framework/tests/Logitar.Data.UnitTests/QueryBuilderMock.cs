namespace Logitar.Data;

internal class QueryBuilderMock : QueryBuilder
{
  public QueryBuilderMock(TableId source) : base(source)
  {
    Dialect.JoinClauses[JoinKind.Full] = "JOINDRE COMPLÈTEMENT";
    Dialect.JoinClauses[JoinKind.Inner] = "JOINDRE À L'INTÉRIEUR";
    Dialect.JoinClauses[JoinKind.Left] = "JOINDRE À GAUCHE";
    Dialect.JoinClauses[JoinKind.Right] = "JOINDRE À DROITE";

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
