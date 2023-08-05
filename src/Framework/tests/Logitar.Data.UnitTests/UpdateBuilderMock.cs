namespace Logitar.Data;

internal class UpdateBuilderMock : UpdateBuilder
{
  public UpdateBuilderMock(TableId source) : base(source)
  {
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

  protected override string AllColumnsClause => "Ω";

  protected override string UpdateClause => "MODIFIER";
  protected override string SetClause => "DÉFINIR";

  protected override string WhereClause => "OÙ";
  protected override string IsClause => "EST";
  protected override string NotClause => "NON";
  protected override string BetweenClause => "DANS L'INTERVALLE";
  protected override string InClause => "DANS";
  protected override string LikeClause => "COMME";
  protected override string NullClause => "NUL";

  protected override object CreateParameter(IParameter parameter) => parameter;
}
