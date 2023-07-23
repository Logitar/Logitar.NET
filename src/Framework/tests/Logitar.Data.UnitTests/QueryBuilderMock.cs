namespace Logitar.Data;

internal class QueryBuilderMock : QueryBuilder
{
  public QueryBuilderMock(TableId source) : base(source)
  {
    JoinClauses[JoinKind.Full] = "JOINDRE COMPLÈTEMENT";
    JoinClauses[JoinKind.Inner] = "JOINDRE À L'INTÉRIEUR";
    JoinClauses[JoinKind.Left] = "JOINDRE À GAUCHE";
    JoinClauses[JoinKind.Right] = "JOINDRE À DROITE";

    ComparisonOperators["="] = "==";
    ComparisonOperators[">"] = ">>";
    ComparisonOperators[">="] = ">>==";
    ComparisonOperators["<"] = "<<";
    ComparisonOperators["<="] = "<<==";
    ComparisonOperators["<>"] = "!=";

    GroupOperators["AND"] = "ET";
    GroupOperators["OR"] = "OU";
  }

  protected override string? DefaultSchema => "défaut";
  protected override string? IdentifierPrefix => "«";
  protected override string? IdentifierSuffix => "»";
  protected override string IdentifierSeparator => "·";
  protected override string? ParameterPrefix => "Π";
  protected override string? ParameterSuffix => "Θ";

  protected override string SelectClause => "SÉLECTIONNER";
  protected override string AllColumnsClause => "Ω";

  protected override string FromClause => "DEPUIS";

  protected override string OnClause => "SUR";

  protected override string WhereClause => "OÙ";
  protected override string IsClause => "EST";
  protected override string NotClause => "NON";
  protected override string BetweenClause => "DANS L'INTERVALLE";
  protected override string InClause => "DANS";
  protected override string LikeClause => "COMME";
  protected override string NullClause => "NUL";

  protected override string OrderByClause => "ORDONNER PAR";
  protected override string ThenByClause => "PUIS PAR";
  protected override string AscendingClause => "↑";
  protected override string DescendingClause => "↓";

  protected override object CreateParameter(IParameter parameter) => parameter;
}
